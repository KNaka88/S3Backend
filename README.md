# S3 Backend

This repo shows an example of using presigned url with multi part upload.


### How to start?

1. Clone the repo
2. Go to the root folder
3. run `docker-compose up -d`
4. Open https://localhost:44362/swagger/index.html
5. You should be able to see the Swagger UI


### How to use my AWS Account instead of Localstack?

1. Create `docker-compose.override.yml` on root folder.
2. Add followings

```
version: '3.4'
services:
  s3backend:
    environment:
      - USE_LOCAL_STACK=false
      - AWS_ACCESS_KEY=YOUR_ACCESS_KEY
      - AWS_SECRET_KEY=YOUR_SECRET_KEY
```


### How to implement frontend?

Coming soon...
