# .NET 8 REST API + Angular Demo

A small .NET 8 REST API designed to work with the Angular Docker sample.

## API endpoints

GET `/`

GET `/api/products`

GET `/api/products/{id}`

POST `/api/products`

Example POST body:

```json
{
  "name": "Headset",
  "price": 4500
}
```

## Run locally

```bash
dotnet restore
dotnet run
```

The API normally runs on an address shown by ASP.NET Core, for example:

```text
http://localhost:5000
```

## Run with Docker

Build:

```bash
docker build -t dotnet-angular-api .
```

Run:

```bash
docker run --rm -p 8080:8080 dotnet-angular-api
```

API:

```text
http://localhost:8080/api/products
```

## Connect from Angular

For an Angular application running directly on the host:

```typescript
private apiUrl = 'http://localhost:8080/api';

getProducts() {
  return this.http.get<Product[]>(`${this.apiUrl}/products`);
}
```

Example Angular model:

```typescript
export interface Product {
  id: number;
  name: string;
  price: number;
}
```

## Docker networking

If Angular and this API are both Docker containers on the same Docker network, do not use
`localhost` from the Angular container to reach the API.

Create a network:

```bash
docker network create demo-network
```

Run the API:

```bash
docker run -d   --name demo-api   --network demo-network   -p 8080:8080   dotnet-angular-api
```

Then configure the Angular/Nginx setup to proxy API requests to:

```text
http://demo-api:8080
```

This is the preferred pattern when both applications are containerized.
