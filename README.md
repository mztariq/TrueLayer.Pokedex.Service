# TrueLayer.Pokedex.Service

<img src="https://user-images.githubusercontent.com/8701702/138789948-ef341bbd-6499-43fd-96f6-777bf0ff4801.png" width="500px" height="120px" alt="some_text">

## To run the application you can use few different tools. 

### Dockerised app
To run the app as dockerised image go to the root directory of where you have cloned the repo from command line/powershell/vscode terminal, etc. 

- Run below command to build an image
``` docker run --rm -it  -p 5000:80/tcp truelayer-pokedex-service:latest   ```
![Screenshot 2021-10-26 at 01 08 06](https://user-images.githubusercontent.com/8701702/138789251-670480a8-cbb4-4578-83cd-698a16c769f9.jpg)


- Run below command to run app 
``` docker run --rm -it  -p 5000:80/tcp truelayer-pokedex-service:latest ```
![Screenshot 2021-10-26 at 01 10 08](https://user-images.githubusercontent.com/8701702/138789270-4c890a64-6dc6-45ae-b6e6-9f089182da2a.jpg)


- Access the browser on this address http://localhost:5000/swagger/ to view swagger or you can simply use the HttPie to test the API. (below we have section to show to to test using HttPie as well)


### Visual Studio 2021
In visual studio you can open the .sln (solution file) file and set the Api project as startup as below steps:

1- Api project
> ![Screenshot 2021-10-26 at 01 13 03](https://user-images.githubusercontent.com/8701702/138787717-ae0c7fa5-f918-4819-b0e8-8b1c0f825f3d.jpg)

2- Set as startup
> ![Screenshot 2021-10-26 at 01 13 19](https://user-images.githubusercontent.com/8701702/138787732-3c6cb49c-718a-4a82-ae0a-afa78c3e274a.jpg)

3- Run the application either using IS express or Kestrel as below option (which should launch the api's swagger docs
> ![Screenshot 2021-10-26 at 01 13 37](https://user-images.githubusercontent.com/8701702/138787785-1336c554-6b83-44e4-af38-ff675624b254.jpg)

4- Swagger docs
> ![Screenshot 2021-10-26 at 01 17 15](https://user-images.githubusercontent.com/8701702/138787840-39bca27c-05ef-419a-9f4d-1acee16185b0.jpg)

You can also use swagger to to the testing of the Api of can use Httpie for command line testing. 

HttPie testing results as below:
> ![TrueLayer Pokedex](https://user-images.githubusercontent.com/8701702/138787994-e5e49a85-858f-4907-89c8-d4c8afb8dc69.PNG)


## VS Code

You can also use vs code to run the application. Easiest way is to install docker extension and build docker image from there. 




## Future Improvements 

There are few improvements which I can think are important to go live with this service. 

- BDD Test (specflow, cucumber, etc.)
- Add more loggin
- Add resilience (polly to check api down and circut breaker)
- Add Caching (redis cache or local storage cache)
- Authentication/Authorization (depends how sensitive data it is, P1, P2, etc.)
- CI/CD deployment using Github actions
- Rate limiter  (how many http call per sec or minutes can go, etc.)
