WLI.JwtAuthentication :
======================
The **WLI.JwtAuthentication** Library for .NET Core provides easy-to-use methods for generating JSON Web Tokens (JWT) and adding JWT-based authentication to your applications. This library simplifies the implementation of secure authentication and authorization mechanisms.

Features :
-----------
- Generate JWT tokens with customizable parameters.
- Refresh Jwt tokens with customizable parameters.
- Integrate JWT-based authentication into your application's services.

`JwtTokenOptions` Class :
=====================
The `JwtTokenOptions` class, located in the **WLI.JwtAuthentication** namespace, provides a streamlined way to configure JWT authentication settings for use with the Token Generation Library. This class encapsulates various options that can be customized according to your application's needs.

### Properties :

- `Key`: The secret key used for token signing. This key ensures the authenticity and integrity of your tokens. It's recommended to use a strong and securely managed key.
- `Issuer`: The issuer of the token, indicating the entity responsible for generating and signing the token. This value helps verify the token's source.
- `Audience`: The intended audience for the token, specifying who the token is intended for. This value can help prevent tokens from being misused by unintended recipients.
- `ValidateActor`: A boolean indicating whether to validate the token actor. The token actor represents the user on whose behalf the token is issued. Enabling this option adds an extra layer of validation. By default its value is true if you want then you can make it false.
- `ValidateIssuer`: A boolean indicating whether to validate the token issuer. Validation ensures that the token's issuer matches the expected value. By default its value is true if you want then you can make it false.
- `ValidateAudience`: A boolean indicating whether to validate the token audience. Validation ensures that the token's audience matches the intended value. By default its value is true if you want then you can make it false.
- `ValidateLifetime`: A boolean indicating whether to validate the token's expiration time. Enabling this option helps ensure that expired tokens are rejected. By default its value is true if you want then you can make it false.
- `ValidateIssuerSigningKey`: A boolean indicating whether to validate the issuer signing key. This validation confirms that the key used for token signing is valid and authorized. By default its value is true if you want then you can make it false.
- `RequireExpirationTime`: A boolean indicating whether to require an expiration time in the token. Enabling this option helps prevent tokens from being used indefinitely. By default its value is true if you want then you can make it false.
- `Events`: Allows you to specify custom event handlers for JWT authentication events.

`GenerateJwtTokenOptions` Class :
==================================
The `GenerateJwtTokenOptions` class, extending the `CommonOptions` class from the `WLI.JwtAuthentication.JwtTokenOptions` namespace, provides a comprehensive set of options for generating JWT tokens using the Token Generation Library. These options allow you to customize various aspects of token generation, such as token expiration times, claims, and more.

### Properties :

- `Key`, `Issuer`, `Audience`, `ValidateActor`, `ValidateIssuer`, `ValidateAudience`, `ValidateLifetime`, `ValidateIssuerSigningKey`, and `RequireExpirationTime`: Inherited from the `CommonOptions` class, these properties allow you to configure the common JWT authentication settings for token generation.

- `Email`: The email associated with the user for whom the token is being generated. This can be used to include user-specific information in the token payload.

- `RefreshToken`: The refresh token used for generating a new access token. This allows you to refresh an access token without requiring the user to log in again.

- `Role`: The role or roles assigned to the user. Roles can be used to implement fine-grained authorization within your application.

- `AccessTokenExpirationTime`: The expiration time (in seconds) for the access token. This defines the period of validity for the access token.

- `RefreshTokenExpirationTime`: The expiration time (in seconds) for the refresh token. This defines the period of validity for the refresh token.

- `Claims`: A list of `Claim` objects that you can use to include additional information in the token payload. Claims represent statements about the user and their roles, rights, or other attributes.

`IJwtTokenService` Interface :
=================================
The IJwtTokenService contains two non static method which is -
- public Tokens JwtToken(GenerateJwtTokenOptions options);
- public Tokens RefreshAccessToken(GenerateJwtTokenOptions options);

`JwtTokenService` Class :
============================
The `JwtTokenService` class in the **WLI.JwtAuthentication** library serves as a core component for generating and refreshing JSON Web Tokens (JWT). This class provides methods to create both access tokens and refresh tokens with customizable parameters. This class is Inherited from IJwtTokenGeneration interface.

## Registering the IJwtTokenService Service :
To utilize the `IJwtTokenService` service in your ASP.NET Core application, you need to register it in the `Program.cs` file. This service is responsible for generating and refreshing JWT tokens.

Here's how you can register the `IJwtTokenService` service in your `Program.cs` file:
```bash
    builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
```

### The Methods which are present inside the class are :
- JwtToken.
- RefreshAccessToken.

JwtToken Method :
==================
In `JwtTokenService` class there is an non static method which name is **JwtToken**. It will returns an Token object which contains access Jwt Token and refresh JWT Token. We have to set the parameter for it.

### Parameters
- **options:** An instance of the `GenerateJwtTokenOptions` class containing the necessary parameters for token generation:
   - `Claims:` The claims to include in the token payload.
   - `AccessTokenExpirationTime:` The expiration time for the access token(in minutes).
   - `RefreshTokenExpirationTime:` The expiration time for the refresh token(in minutes).
   -  `Key:` The secret key used for token signing.
   -  `Audience:` The intended audience for the tokens.
   -  `Issuer:` The issuer of the tokens.

### Exmaple to call this method :
- Create instance of `IJwtTokenService` like _jwtTokenService.
- For getting value of `Key`, `Issuer` and `Audeince` create an instance of Configurable class like _config.

- You can get you `Key`, `Issuer`, `Audience` from your appsettings.json file.
```bash
 var options = new GenerateJwtTokenOptions
 {
     Email = "test@gmail.com",
     Issuer = _config.GetSection("Jwt:Issuer").Value, // Get your Issuer from your appsettings.json file e.g. "https://localhost:7129"
     Audience = _config.GetSection("Jwt:Audience").Value, // Get your Audience from your appsettings.json file e.g. "https://localhost:7129"
     Key = _config.GetSection("Jwt:Key").Value // Get your Key from your appsettings.json file e.g. key:4F9A5D6E8B1C2F7D9A5D6E8B1C2F
     Role = "TestRole",
     AccessTokenExpirationTime = Convert.ToInt32(2),//In Minutes
     RefreshTokenExpirationTime = Convert.ToInt32(5),//In Minutes
     Claims = new List<Claim>
     {
         new Claim(ClaimTypes.Email, "test@gmail.com"),
         new Claim(ClaimTypes.Role, "TestRole")
     }
     // You can specific validations for it from CommonOptions class
     ValidateIssuer = true;
     ValidateAudience = ture;
     Event = new JwtBearerEvents(){
          OnChallenge = async context =>
          {
            context.HandleResponse();
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";
            
            await context.Response.WriteAsync(JsonConvert.SerializeObject("Custom object", jsonSerialize));
         }
     }
 };
```

### Method Calling :
    _jwtTokenService.JwtToken(options);
### Method Signature :
        public Tokens JwtToken(GenerateJwtTokenOptions options);

### Installation:
- Open your project in Visual Studio or your preferred code editor.

- Add a reference to the JWT Token Authentication Library in your project.
- Import the library namespace in the classes where you want to use it.
   - using WLI.JwtAuthentication;
- Create instance of IJwtTokenService like _jwtTokenService
- Call the Method using Class Name.
   - _jwtTokenService.JwtToken(options);

### Return Value :
The method returns a Tokens object containing the newly generated access token and refresh token.

RefreshAccessToken  Method :
============================
The `RefreshAccessToken` method in the **WLI.JwtAuthentication** library is responsible for refreshing an access token using a provided refresh token. This method simplifies the process of renewing tokens without the need for users to re-authenticate.

### Parameters :
- **options:** An object of type GenerateJwtTokenOptions that includes the required parameters for token generation:
   - `Claims:` The claims to be included in the token payload.
   - `AccessTokenExpirationTime:` The access token's expiration time.
   - `RefreshTokenExpirationTime:` The refresh token's expiration time.
   -  `Key:` The secret key for token signing.
   -  `Audience:` The intended audience for the tokens.
   -  `Issuer:` The issuer of the tokens.

### Exmaple to call this method : 
- Create instance of `IJwtTokenService` like _jwtTokenService.
- For getting value of `Key`, `Issuer` and `Audeince` create an instance of Configurable class like _config.
```bash
var options = new GenerateJwtTokenOptions
{
    RefreshToken = refreshToken,
    Issuer = _config.GetSection("Jwt:Issuer").Value, // Get your Issuer from your appsettings.json file e.g. "https://localhost:7129"
    Audience = _config.GetSection("Jwt:Audience").Value, // Get your Audience from your appsettings.json file e.g. "https://localhost:7129"
    Key = _config.GetSection("Jwt:Key").Value // Get your Key from your appsettings.json file e.g. key:4F9A5D6E8B1C2F7D9A5D6E8B1C2F
    AccessTokenExpirationTime = Convert.ToInt32(2),//In Minutes
    RefreshTokenExpirationTime = Convert.ToInt32(5),//In Minutes
};
```
### Method Calling :
    _jwtTokenService.RefreshAccessToken(options);

### Method Signature :
        public Tokens RefreshAccessToken(GenerateJwtTokenOptions options);

## Return Value :
The method returns a Tokens object containing the newly generated access token and refresh token.

`JwtAuthentication` Class :
===========================
In JwtAuthentication Class there is an static method which name is AddJwtAuthentication().The AddJwtAuthentication method simplifies the process of adding JWT-based authentication to your application's services using the ASP.NET Core IServiceCollection.

### Adding JWT Authentication in Program.cs :
- Import the library namespace in the Program.cs class.
    - using WLI.JwtAuthentication;
```bash
var jwtOptions = new JwtTokenOptions
{
    // Set your JWT configuration options here
    ValidateActor = true,
    ValidateAudience = true,
    RequireExpirationTime = true,
    ValidateIssuerSigningKey = true,
    Issuer = builder.Configuration.GetSection("Jwt:Issuer").Value, // Get your Issuer from your appsettings.json file
    Audience = builder.Configuration.GetSection("Jwt:Audience").Value, // Get your Audience from your appsettings.json file
    Key = builder.Configuration.GetSection("Jwt:Key").Value // Get your Key from your appsettings.json file
};
```
### Calling Method :
services.AddJwtAuthentication(jwtOptions);

### Method Signature :
      public static void AddJwtAuthentication(this IServiceCollection services, JwtTokenOptions jwtOptions);

### Security Considerations :
- **Secret Key:** Keep your secret key confidential and avoid hardcoding it in your codebase.
- **Audience and Issuer:** Ensure that the values for audience and issuer match the ones used during token generation.

  ## Want to Contribute?

- Created something awesome, made this code better, added some functionality, or whatever (this is the hardest part).
- [Fork it](http://help.github.com/forking/).
- Create new branch to contribute your changes.
- Commit all your changes to your branch.
- Submit a [pull request](http://help.github.com/pull-requests/).

---

## Collection of Components

We have built many other components and free resources for software development in various programming languages. Kindly click here to view our [Free Resources for Software Development](https://www.weblineindia.com/software-development-resources.html)

---

## License

[MIT](LICENSE)

[mit]: https://github.com/weblineindia/ReactJS-Email/blob/master/LICENSE

## Keywords
 
dotnet, jwt, authentication, login
