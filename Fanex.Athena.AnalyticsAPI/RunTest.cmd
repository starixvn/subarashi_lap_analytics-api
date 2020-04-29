dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover,cobertura\" /p:ExcludeByFile=\"../Fanex.Athena.AnalyticsAPI.Common.Resources/**/*.cs\" /p:ExcludeByAttribute=\"Obsolete,GeneratedCodeAttribute,CompilerGeneratedAttribute\"
dotnet ".\reportgenerator\4.2.19\tools\netcoreapp2.1\ReportGenerator.dll" "-reports:.\**\*.opencover.xml" "-targetdir:..\TestReports"
