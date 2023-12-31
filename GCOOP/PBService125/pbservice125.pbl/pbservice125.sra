$PBExportHeader$pbservice125.sra
forward
global type pbservice125 from Application
end type
global Transaction sqlca
global DynamicDescriptionArea sqlda
global DynamicStagingArea sqlsa
global Error error
global Message message
end forward

global variables
String ls_suffix=""
end variables

global type pbservice125 from Application
string AppName = "pbservice125"
end type
global pbservice125 pbservice125

on pbservice125.create
sqlca=create Transaction
sqlda=create DynamicDescriptionArea
sqlsa=create DynamicStagingArea
error=create Error
message=create Message
end on

on pbservice125.destroy
destroy(sqlca)
destroy(sqlda)
destroy(sqlsa)
destroy(error)
destroy(message)
end on
