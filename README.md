# Rabbitmq_Example
<h4>Set up RabbitMQ message broker with Asp.Net Core 3.1 and Docker.</h4>

This project is an example of how to run a RabbitMQ in Asp.Net Core 3.1 .

For install and run <b>RabbitMQ Management</b> by Docker,run the following code in the terminal:

<code>sudo docker run --rm -p 5672:5672 -p 15672:15672 --name bitnami bitnami/rabbitmq</code>

for see rabbitmq managment, call <b>localhost:15672</b> in browser.

<b>username:</b> user

<b>password:</b> bitnami

Url for calling apis:

<b>localhost:7000/Notification/SendEmail</b>

body:
<text>
{
    "subject":"test",
    "message":"Hello amin Golmahalle",
    "Email":"test@gmail.com"
}
</text>

<b>localhost:7000/Notification/SendSms</b>

body:
{
    "message":"Hello amin Golmahalle",
    "mobile":"09123456789"
}
