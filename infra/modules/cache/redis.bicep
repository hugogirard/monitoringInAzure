param location string

resource redisCache 'Microsoft.Cache/redis@2023-08-01' = {
  name: 'rediscachedemohg'
  location: location
  properties: {
    sku: {
      name: 'Basic'
      family: 'C'
      capacity: 0
    }
    enableNonSslPort: false
    minimumTlsVersion: '1.2'
  }
}
