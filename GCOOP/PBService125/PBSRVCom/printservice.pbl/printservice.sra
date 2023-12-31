$PBExportHeader$printservice.sra
$PBExportComments$Generated Application Object
forward
global type printservice from application
end type
global transaction sqlca
global dynamicdescriptionarea sqlda
global dynamicstagingarea sqlsa
global error error
global message message
end forward

global type printservice from application
string appname = "printservice"
end type
global printservice printservice

on printservice.create
appname = "printservice"
message = create message
sqlca = create transaction
sqlda = create dynamicdescriptionarea
sqlsa = create dynamicstagingarea
error = create error
end on

on printservice.destroy
destroy( sqlca )
destroy( sqlda )
destroy( sqlsa )
destroy( error )
destroy( message )
end on

