using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyChangeControlLib.Types;
using SpotifyChangeControlLib.DatabaseObjects;

namespace SpotifyChangeControlLib.DataObjects
{
    /// <summary>
    /// Because of the SpotifyID being a 500 character guid 
    /// There is special consideration and logic required 
    /// for the tables to be accessed and used in a manner that
    /// doesn't cause the database to set itself on fire in protest
    /// of indexing  guid. 
    /// 
    /// The basic idea is when the spotifydatabase class is asked to do a
    /// builkInsert into the WK tables it will ensure the WK  table has been truncated and
    /// indexes has been disabled. Once the insert completes the indexes will be rebuilt
    /// A procedure will then move from WK to our permanent table and then those indexes will need
    /// to be rebuilt. And then repeat!
    /// </summary>
    internal class TableState
    {
        //Declare some static things for everyone to share
        private static readonly string _TruncateCommandTemplate = "TRUNCATE TABLE [{SchemaName}].[{TableName}];";
        private static readonly string _HeapCommandTemplate = "ALTER INDEX ALL ON [{SchemaName}].[{TableName}] DISABLE;";
        private static readonly string _IndexCommandTemplate = "ALTER INDEX ALL ON [{SchemaName}].[{TableName}] REBUILD;";
        //Instance Variables
        private string _TableName;
        private string _SchemaName;
        private bool _WorkTable;
        private Enums.DatabaseTableState _TableState;

        private string _TruncateCommand
        {
            get { return TableState._TruncateCommandTemplate.Replace("{SchemaName}", this._SchemaName).Replace("{TableName}", this._TableName); }
        }
        private string _HeapCommand
        {
            get { return TableState._HeapCommandTemplate.Replace("{SchemaName}", this._SchemaName).Replace("{TableName}", this._TableName); }
        }
        private string _IndexCommand
        {
            get { return TableState._IndexCommandTemplate.Replace("{SchemaName}", this._SchemaName).Replace("{TableName}", this._TableName); }
        }
        public string TableName
        {
            get { return this._TableName; }
        }

        //Constructor :)
        public TableState(string sTableName, string sSchemaName = "dbo")
        {
            this._TableName = sTableName;
            this._SchemaName = sSchemaName;
            this._TableState = Enums.DatabaseTableState.Unknown;
            if (sTableName.Contains("WK"))
            {
                this._WorkTable = true;
            }
            else
            {
                this._WorkTable = false;
            }
        }

        private void TruncateTable()
        {
            if ( (this._TableState & Enums.DatabaseTableState.Truncated) != Enums.DatabaseTableState.Truncated) //Check our bitwise flag of enums to make sure the table has not already been truncated
            {
                if (this._WorkTable)
                {
                    SpotifyDatabase.ExecuteNonQuery(this._TruncateCommand);
                }
            }
            
        }

        private void HeapTable()
        {
            if ((this._TableState & Enums.DatabaseTableState.Heaped) != Enums.DatabaseTableState.Heaped) //Check our bitwise flag of enums to make sure the table has not already been heaped
            {
                if (this._WorkTable)
                {
                    SpotifyDatabase.ExecuteNonQuery(this._HeapCommand);
                }
            }
        }

        private void IndexTable()
        {
            if ((this._TableState & Enums.DatabaseTableState.Querable) != Enums.DatabaseTableState.Querable) //Check our bitwise flag of enums to make sure the table has not already been indexed
            {
                SpotifyDatabase.ExecuteNonQuery(this._IndexCommand);
            }
        }

    }
}
