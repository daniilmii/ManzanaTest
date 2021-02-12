В репозитории 2 сервиса.

1)CheckMonitoringService.
 Windows служба, которая мониторит указанную в конфиге (conf.json) директорию - CheckFolderPath.

- Система проверяет директории "ChecksFolderPath","GarbageFolderPath","CompleteFolderPath" на существование. 
Если какие-либо пути к директориям некорректные или не существуют, служба не запустится.
Также проверяются все остальные поля в конфиге на корректность, если хотябы одно некорректное, то  служба не запустится.

- Файлы в директории проверяются на расширения. Обрабатываются только (.txt). Все остальные файлы улетают в папку Garbage.
Корректные файлы отправляются в CheckServiceWCF методом (/PostCheck) по адресу указанному в конфиге - HostIp:HostPort.
Если запрос выполнился без ошибок, то файл перемещается в папку CompleteFolderPath.
Если с ошибками , файл перемещается в GarbageFolderPath.

-Службу можно запускать для отладки в консоли. По нажатию на любую клавишу, она завершится.

-В сервисе есть другой метод "MonitoringDirectoryWeak()" с событием OnCreated для мониторинга дериктории, но у него есть ограничения, которые могут привести к неправильной работе системы при большом количестве файлов, 
поэтому был реализован метод MonitoringDirectory().

2)CheckServiceWCF

-Также выполняется проверка конфигов и сервис не запускается в случае некорректых данных


- В сервисе есть 2 метода:
  void PostCheck(); получает корректный чек от службы и заливает в базу 
  string GetChecks(string count); выдает последние n - чеков из базы 

//English version

There are 2 services in the repository.
1) CheckMonitoringService.
 Windows service that monitors the directory specified in the config (conf.json) - CheckFolderPath.

- The system checks the "ChecksFolderPath", "GarbageFolderPath", "CompleteFolderPath" directories for existence.
If any directory paths are invalid or do not exist, the service will not start.
Also, all other fields in the config are checked for correctness, if at least one is incorrect, then the service will not start.

- Files in the directory are checked for extensions. Only (.txt) are processed. All other files go to the Garbage folder.
Correct files are sent to CheckServiceWCF using the (/ PostCheck) method at the address specified in the config - HostIp: HostPort.
If the request was completed without errors, then the file is moved to the CompleteFolderPath folder.
If it fails, the file is moved to the GarbageFolderPath.

-The service can be started for debugging in the console. By pressing any key, it will end.

-The service has another method "MonitoringDirectoryWeak ()" with the OnCreated event to monitor the directory, but it has limitations that can lead to incorrect system operation with a large number of files,
therefore the MonitoringDirectory () method was implemented.

2) CheckServiceWCF

-Also, the configs are checked and the service does not start in case of incorrect data


- The service has 2 methods:
  void PostCheck (); receives the correct check from the service and uploads it to the database
  string GetChecks (string count); issues the last n - checks from the database
