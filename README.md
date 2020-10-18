# JsonPlaceHolder
     Criteria to Implement
The only criteria I did not implement was the log/html report. I used the Visual Studio IDE to create the API tests along with the Nunit library among others. The debugging functionality in that 
environment was enough for me to debug any issues with the project. 

      Architectural issues
I notice several things with the API. 

1. No Authentication - no token, no API Key, no security
2. There did not seem to be any required fields in the payloads. Ex. the For Create_A_JSON_Post API Test, I could create a post without body,title
3. No validation on the fields
  a. I could pass characters such as '@#$^&*?<>|:+-=!()' into the payloads
  b. I could pass in a negative integer for userId ex. -10 with no error message
  c. I could pass in string user id which was then turned into a 0 with no error message.
  d. I could pass in an invalid email format, c_jung.yahoo in Create_A_Json_Post_With_Comments.

4. No payload has to be passed to create a post or a post comment. 
