команды consul

запуск агента
c:\consul.exe agent -data-dir=/cconf

c:\consul.exe agent -dev -ui

сохранить значение
consul kv put conf3/auditSrv/clkTable St_TracingTable

получает значения
consul kv get -recurse