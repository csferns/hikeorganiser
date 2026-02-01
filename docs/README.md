# Project structure

* src
  * HikeOrganiser.API
  * HikeOrganiser.Core
  * HikeOrganiser.Data
  * vite-project
    * src
      * 
* test
  * HikeOrganiser.Tests
    * Unit test / integration testing project
* docs
  * Various documentation related to the project(s)

# Running the project

### running the api
[dotnet 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

```pwsh
cd src/HikeOrganiser.API
dotnet run -c Release 
```

Running this will give you what address/port the api is running on, e.g.
![img.png](img.png)

### running the web
You need [node js](https://nodejs.org/en/download/current) to be installed

```pwsh
cd src/vite-project/src
npm install
```

once it's installed, you need to set the api endpoint for the pages to access the api. This can be done with:

```pwsh
set "REACT_APP_API_ENDPOINT=https://localhost:7259"
```

once this has been set, you can run the app with

```pwsh
npm run dev
```

e.g.

![img_1.png](img_1.png)