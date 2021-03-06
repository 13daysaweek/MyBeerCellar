param location string = resourceGroup().location
param webAppName string
param keyVaultName string
param tenantId string
param ownerObjectId string
param sqlServerName string = 'mybrewcellarsqlsrv'
param sqlDbName string = 'MyBrewCellarDb'
param sqlAdminLogin string
param sqlAdminPassword string
param environmentSuffix string


var farmName = '${webAppName}-farm'
var sqlServerFullName = '${sqlServerName}-${environmentSuffix}'
var appInsightsName = '${webAppName}-insights'
var appConfigName = 'mybeercellar-config-${environmentSuffix}'

resource keyVault 'Microsoft.KeyVault/vaults@2016-10-01' = {
    name: keyVaultName
    location: location
    properties: {
        sku: {
            family: 'A'
            name: 'Standard'
        }
        tenantId: tenantId
        accessPolicies: [
            {
                tenantId: tenantId
                objectId: ownerObjectId
                permissions: {
                    keys: [
                        'Get'
                        'List'
                        'Update'
                        'Create'
                        'Import'
                        'Delete'
                        'Recover'
                        'Backup'
                        'Restore'
                    ]
                    secrets: [
                        'Get'
                        'List'
                        'Set'
                        'Delete'
                        'Recover'
                        'Backup'
                        'Restore'
                    ]
                    certificates: [
                        'Get'
                        'List'
                        'Update'
                        'Create'
                        'Import'
                        'Delete'
                        'Recover'
                        'Backup'
                        'Restore'
                        'ManageContacts'
                        'ManageIssuers'
                        'GetIssuers'
                        'ListIssuers'
                        'SetIssuers'
                        'DeleteIssuers'
                    ]
                }
            }
        ]
    }
}

resource appConfig 'Microsoft.AppConfiguration/configurationStores@2020-06-01' = {
    name: appConfigName
    location: location
    sku: {
        name: 'free'
    }
    properties: {
        encryption: {}
    }
}

resource sqlServer 'Microsoft.Sql/servers@2019-06-01-preview' = {
    name: sqlServerFullName
    location: location
    kind: 'v12.0'
    properties: {
        administratorLogin: sqlAdminLogin
        administratorLoginPassword: sqlAdminPassword
        version: '12.0'
        publicNetworkAccess: 'Enabled'
    }
}

resource sqlDb 'Microsoft.Sql/servers/databases@2019-06-01-preview' = {
    name: '${sqlServer.name}/${sqlDbName}'
    location: location
    sku: {
        name: 'Standard'
        tier: 'Standard'
        capacity: 50
    }
    kind: 'v12.0,user'
    properties: {
        collation: 'SQL_Latin1_General_CP1_CI_AS'
        catalogCollation: 'SQL_Latin1_General_CP1_CI_AS'
        zoneRedundant: false
        licenseType: 'LicenseIncluded'
        readScale: 'Disabled'
        readReplicaCount: 0
        storageType: 'GRS'
    }
}

resource tde 'Microsoft.Sql/servers/databases/transparentDataEncryption@2014-04-01-preview' = {
    name: '${sqlDb.name}/current'
    properties: {
        status: 'Enabled'
    }
}

resource allowAzureIps 'Microsoft.Sql/servers/firewallRules@2015-05-01-preview' = {
    name: '${sqlServerFullName}/AllowAllWindowsAzureIps'
    properties: {
        startIpAddress: '0.0.0.0'
        endIpAddress: '0.0.0.0'
    }
}

resource webApp 'Microsoft.Web/sites@2018-11-01' = {
  name: webAppName
  location: location
  identity: {
      type: 'SystemAssigned'
  }
  properties: {
      name: webAppName
      serverFarmId: farm.id
      siteConfig: {
          appSettings: [
          ]
          connectionStrings: [
            {
                name: 'AppConfig'
                connectionString: appConfig.properties.endpoint
            }
          ]
      }
  }
}

resource farm 'Microsoft.Web/serverFarms@2018-11-01' = {
    name: farmName
    location: location
    sku: {
        name: 'S1'
        tier: 'Standard'
    }
    kind: 'linux'
    properties: {
        name: farmName
        workerSize: '0'
        workerSizeId: '0'
        numberOfWorkers: '1'
        reserved: true
    }
}

resource appInsights 'microsoft.insights/components@2018-05-01-preview' = {
    name: appInsightsName
    location: location
    kind: 'web'
    properties: {
        Application_Type: 'web'
        Request_Source: 'rest'
        RetentionInDays: 90
        publicNetworkAccessForIngestion: 'Enabled'
        publicNetworkAccessForQuery: 'Enabled'
    }
}

output appConfigResourceName string = appConfig.name
output resourceGroupName string = resourceGroup().name
output appServiceResourceName string = webApp.name
output sqlServerResourceName string = sqlServerFullName
output sqlDbResourceName string = sqlDb.name