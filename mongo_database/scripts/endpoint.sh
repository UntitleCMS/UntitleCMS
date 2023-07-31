#!/usr/bin/bash

LOG_FILE=/data/db/log/script.log.txt
PORT=50001
HOST=mongo1

print_head(){
    echo ""
    echo "========================$*========================"
}
print_fail(){
    echo ""
    echo "!!!!!!!!!!!!!!!!!!!!!!!!$*!!!!!!!!!!!!!!!!!!!!!!!!"
}

check_connection(){
    print_head 'CHECK CONNECTION MONGO1'
    sleep 3
    mongosh --port $PORT
}

initrs() {

    if [ "$IS_PRIMARY" != "true" ]
    then
        print_head "NOT_PRIMARY"
        return 1
    fi

    print_head 'PRIMARY SETUP'
    until check_connection
    do
        print_fail CONNECTION FAILED
    done
    echo ""

    print_head 'INIT RS SCRIPT'
    mongosh --port $PORT < /scripts/init.js
}

# INIT RS SCRIPT
initrs > $LOG_FILE &

# set key to mongodb
chown mongodb:mongodb /scripts/mongo-keyfile

# START DATABASE
# mongod --keyFile /scripts/mongo-keyfile --bind_ip_all --replSet "my-mongo-set"


mongod --port $PORT --replSet "my-mongo-set" --bind_ip_all
