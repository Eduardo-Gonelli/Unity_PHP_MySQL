# Unity_PHP_MySQL
Simple project to make a PHP service call, which accesses a MySQL database and returns the data in JSON format.

To use it, you need to create the PHP services that return, either from a MySQL database or from another database, the data in JSON format:

[{"id":"1","name":"Ed","email":"edu@gmail.com","password":"hashSha256EncryptedString"},{"id":"1","name ":"Ed","email":"edu@gmail.com","password":"hashSha256EncryptedString"}]

and so on.

It has four classes:
PlayerData: base for player data;
DataManager: passes the data to a PHP service and receives the data;
ByPassHTTPSCertificate: used if your server does not support HTTPS;
UIManager: Displays the data on the screen by transforming the received json into an object that contains multiple PlayerData.

If you create the necessary services, you can start with the PersistendData scene.

Inside the Scripts folder there is a folder with two PHP example codes.

The MySQL database was configured like this:

Table name: players

Column Name   Datatype       Options
id            INT            PK NN AI
name          VARCHAR(70)    NN
email         VARCHAR(50)    NN
password      VARCHAR(150)   NN
created_at    TIMESTAMP      NN CURRENT_TIMESTAMP


These examples were created for educational purposes and may have security breaches. 
Unless you are sure what you are doing, it is recommended not to use it in production.
