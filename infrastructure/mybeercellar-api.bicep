param location string = resourceGroup().location
param webAppName string
param keyVaultName string
param tenantId string
param ownerObjectId string
param sqlServerName string
param sqlAdminLogin string
param sqlAdminPassword string

var farmName = '${webAppName}-farm'

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

resource sqlServer 'Microsoft.Sql/servers@2019-06-01-preview' = {
    name: sqlServerName
    location: location
    kind: 'v12.0'
    properties: {
        administratorLogin: sqlAdminLogin
        administratorLoginPassword: sqlAdminPassword
        version: '12.0'
        publicNetworkAccess: 'Enabled'
    }
}

resource webApp 'Microsoft.Web/sites@2018-11-01' = {
  name: webAppName
  location: location
  properties: {
      name: webAppName
      serverFarmId: farm.id
      siteConfig: {
          appSettings: [
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