dotnet publish ./MovieCollectionAPI.csproj -c Release -o ./publish

scp -r ./publish/* root@164.92.201.138:/opt/krecim/

ssh root@164.92.201.138 "sudo systemctl restart krecim"

Write-Host "✅ Deployment complete!"