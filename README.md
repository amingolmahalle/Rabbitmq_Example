# Rabbitmq_Example
<h4>Set up RabbitMQ message broker with Asp.Net Core 3.1 and Docker.</h4>

This project is an example of how to run a RabbitMQ in Asp.Net Core 3.1 .

For install and run <b>RabbitMQ Management</b> by Docker,run the following code in the terminal:

<code>sudo docker run -d --rm  --hostname my-rabbit --name rabbitmq-mg -p 15672:15672 -p 5672:5672 rabbitmq:3-management</code>

for see rabbitmq managment, call <b>localhost:15672</b> in browser.
