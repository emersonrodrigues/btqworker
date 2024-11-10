#!/bin/bash

# Espera o RabbitMQ iniciar
sleep 10

# # Criação da fila de Dead Letter
# curl -i -u guest:guest -H "content-type:application/json" \
#   -XPUT -d'{"durable":true}' \
#   http://localhost:15672/api/queues/%2F/deadletter

# # Criação da fila Orders com Dead Letter Exchange configurado
# curl -i -u guest:guest -H "content-type:application/json" \
#   -XPUT -d'{"durable":true, "arguments":{"x-dead-letter-exchange":"", "x-dead-letter-routing-key":"deadletter"}}' \
#   http://localhost:15672/api/queues/%2F/orders



curl -i -u guest:guest -H "content-type:application/json" \
  -XPUT -d'{"type":"direct","durable":true}' \
  http://localhost:15672/api/exchanges/%2F/deadletter-exchange


curl -i -u guest:guest -H "content-type:application/json" \
  -XPUT -d'{"durable":true}' \
  http://localhost:15672/api/queues/%2F/deadletter

# Criar o binding entre a exchange "deadletter-exchange" e a fila "deadletter"
curl -i -u guest:guest -H "content-type:application/json" \
  -XPOST -d'{"routing_key":"deadletter"}' \
  http://localhost:15672/api/bindings/%2F/e/deadletter-exchange/q/deadletter


curl -i -u guest:guest -H "content-type:application/json" \
  -XPUT -d'{"durable":true, "arguments":{"x-dead-letter-exchange":"deadletter-exchange","x-dead-letter-routing-key":"deadletter"}}' \
  http://localhost:15672/api/queues/%2F/orders
