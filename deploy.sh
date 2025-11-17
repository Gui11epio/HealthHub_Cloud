#!/bin/bash

# ================================
# 🚀 DEPLOY HEALTHHUB - AZURE CLI (SQL SERVER)
# ================================

# --- Configurações iniciais ---
RESOURCE_GROUP="rg-healthhub"
LOCATION="brazilsouth"
PLAN_NAME="plan-healthhub"
APP_NAME="healthhub-app"
SQL_SERVER="health-sql-server"
SQL_ADMIN="healthadmin"
SQL_PASSWORD="HealthHub020306!"
SQL_DATABASE="healthdb"

az provider register --namespace Microsoft.Sql

# --- Criar Resource Group ---
echo "📦 Criando Resource Group..."
az group create --name $RESOURCE_GROUP --location $LOCATION

# --- Criar App Service Plan ---
echo "🧱 Criando App Service Plan..."
az appservice plan create   --name $PLAN_NAME   --resource-group $RESOURCE_GROUP   --sku B1

# --- Criar SQL Server ---
echo "🗄️ Criando Servidor SQL..."
az sql server create --name $SQL_SERVER  --resource-group $RESOURCE_GROUP  --location $LOCATION  --admin-user $SQL_ADMIN   --admin-password "$SQL_PASSWORD" --enable-public-network true

# --- Criar Banco de Dados ---
echo "💾 Criando banco de dados..."
az sql db create  --resource-group $RESOURCE_GROUP   --server $SQL_SERVER   --name $SQL_DATABASE  --service-objective Basic --backup-storage-redundancy Local --zone-redundant false

# --- Liberar acesso público (temporário para teste) ---
echo "🌐 Liberando acesso público..."
az sql server firewall-rule create   --resource-group $RESOURCE_GROUP   --server $SQL_SERVER   --name AllowAll   --start-ip-address 0.0.0.0   --end-ip-address 255.255.255.255

# --- Criar o App Service ---
echo "🚀 Criando Web App..."
az webapp create   --resource-group $RESOURCE_GROUP   --plan $PLAN_NAME   --name $APP_NAME   --runtime "dotnet:9"   --deployment-local-git

# --- Configurar Connection String ---
echo "🔗 Configurando Connection String..."
CONNECTION_STRING="Server=tcp:$SQL_SERVER.database.windows.net,1433;Initial Catalog=$SQL_DATABASE;Persist Security Info=False;User ID=$SQL_ADMIN;Password=$SQL_PASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

# Define no Azure App Service (Connection Strings)
az webapp config connection-string set \
  --name $APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --connection-string-type SQLAzure \
  --settings DEFAULT_CONNECTION="$CONNECTION_STRING"


# --- Aplicar migrações do Entity Framework ---
echo "🧩 Aplicando migrações do Entity Framework..."
export DEFAULT_CONNECTION="$CONNECTION_STRING"

az webapp config connection-string list \
  --resource-group rg-healthhub \
  --name healthhub-app


# Certifique-se de que o dotnet-ef está instalado
if ! command -v dotnet-ef &> /dev/null
then
    echo "📦 Instalando dotnet-ef..."
    dotnet tool install --global dotnet-ef
    export PATH="$PATH:$HOME/.dotnet/tools"
fi

# Executar as migrações
dotnet ef database update --project ./Health-Hub.Infrastructure


# --- Finalização ---
echo ""
echo "✅ Deploy concluído com sucesso!"
echo "🌍 Acesse sua aplicação em: https://$APP_NAME.azurewebsites.net"
echo "📜 Logs em tempo real: az webapp log tail --resource-group $RESOURCE_GROUP --name $APP_NAME"

