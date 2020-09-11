param location string = resourceGroup().location
param webAppName string = 'mybeercellarapi'
param keyVaultName string = 'mybrewcellar-vault'
param tenantId string

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