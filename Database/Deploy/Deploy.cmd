@echo off
REM Get Dates in ISO format for later use
for /f "tokens=2 delims==" %%a in ('wmic OS Get localdatetime /value') do set "dt=%%a"
set "YY=%dt:~2,2%" & set "YYYY=%dt:~0,4%" & set "MM=%dt:~4,2%" & set "DD=%dt:~6,2%"
set "HH=%dt:~8,2%" & set "Min=%dt:~10,2%" & set "Sec=%dt:~12,2%"
set "datestamp=%YYYY%%MM%%DD%" & set "timestamp=%HH%%Min%%Sec%"
set "fullstamp=%YYYY%-%MM%-%DD%_%HH%-%Min%-%Sec%"

REM Get Parent and Current Directories for use later on...
SET CurrentDir=%cd%
pushd ..
SET ParentDir=%cd%
popd
SET DeployFile=%CurrentDir%\Deploy.sql



REM Reset file to blank before appending
type nul > %DeployFile%


REM BEGIN FILE HEADER
echo  /* >> %DeployFile%
echo  GeneratedBy: %USERNAME% >> %DeployFile%
echo  GeneratedDate: %datestamp% >> %DeployFile%
echo  */ >> %DeployFile%
REM END FILE HEADER 


echo. >> %DeployFile%
echo. >> %DeployFile%
echo. >> %DeployFile%

REM BEGIN TABLES
echo  /* >> %DeployFile%
echo  BEGIN TABLES >> %DeployFile%
echo  */ >> %DeployFile%
type %ParentDir%\Tables\*.Table.sql >> %DeployFile%
REM END TABLES
echo  /* >> %DeployFile%
echo  END TABLES >> %DeployFile%
echo  */ >> %DeployFile%

echo. >> %DeployFile%
echo. >> %DeployFile%
echo. >> %DeployFile%


REM BEGIN PROCEDURES
echo  /* >> %DeployFile%
echo  BEGIN PROCEDURES >> %DeployFile%
echo  */ >> %DeployFile%
type %ParentDir%\Procedures\*.Procedure.sql >> %DeployFile%
REM END PROCEDURES
echo  /* >> %DeployFile%
echo  END PROCEDURES >> %DeployFile%
echo  */ >> %DeployFile%



echo. >> %DeployFile%
echo. >> %DeployFile%
echo. >> %DeployFile%



REM BEGIN DATA
echo  /* >> %DeployFile%
echo  BEGIN DATA >> %DeployFile%
echo  */ >> %DeployFile%
type %ParentDir%\Data\*.Table.sql >> %DeployFile%
REM END DATA
echo  /* >> %DeployFile%
echo  END DATA >> %DeployFile%
echo  */ >> %DeployFile%

