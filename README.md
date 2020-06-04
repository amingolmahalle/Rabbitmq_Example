# Rabbitmq_Example
Set up RabbitMQ message broker with Asp.Net Core 3.1 and Docker.

This project is an example of how to run a RabbitMQ in Asp.Net Core 3.1 .

for install and run 'RabbitMQ Management' by Docker,Run the following code in the terminal:

sudo docker run -d --rm  --hostname my-rabbit --name rabbitmq-mg -p 15672:15672 -p 5672:5672 rabbitmq:3-management

then You can see table with call localhost:15672 in browser.
