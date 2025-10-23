# Toepen

## Getting started
- clone repository

### On Windows
after running open a reverse proxy for example:
```bash
iisexpress-proxy 5273 to 5555
```

### Deployment
Build dll release file
```bash
dotnet publish --configuration Release
```
Copy release file to server
```bash
scp Toepen_10_Hub/bin/Release/net9.0/publish/* jens@toepenhub.jensramakers.nl:/mnt/volume_ams3_02/apps/toepen/Toepen
```
On the server copy `appsettings.Development.json` to `appsettings.json` and edit the settings
```bash
cp appsettings.Development.json appsettings.json
```

Create a systemd service file `/etc/systemd/system/toepen.service`
```service
[Unit]
Description=Toepen Service

[Service]
WorkingDirectory=/mnt/volume_ams3_02/apps/toepen/Toepen
ExecStart=/usr/bin/dotnet /mnt/volume_ams3_02/apps/toepen/Toepen/Toepen_10_Hub.dll

Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=dotnet-toepenhub
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_NOLOGO=true
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
Environment=ASPNETCORE_URLS=http://localhost:5001

[Install]
WantedBy=multi-user.target
```
Enable and start the service
```bash
sudo systemctl enable toepenhub.service && sudo systemctl start toepenhub.service && sudo systemctl status toepenhub.service
```

In nginx config dont use keep-active, this breaks the websocket instead use this  
```bash
proxy_set_header   Connection $http_connection;
```
Use same port as in the service file above (5001)
```nginx
server {
    ...
    location / {
        ...
        proxy_pass         http://127.0.0.1:5001/;
        ...
    }
    ...
}
```
