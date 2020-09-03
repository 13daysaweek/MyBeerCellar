param location string = resourceGroup().location
param webAppName string = 'mybeercellarapi'

var websiteName = '${webAppName}-site'
var farmName = '${webAppName}-farm'

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