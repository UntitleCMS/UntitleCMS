var config = {
    "_id": "my-mongo-set",
    "version": 1,
    "members": [
        {
            "_id": 1,
            "host": "mongo1:50001",
        }
    ]
};
rs.initiate(config, { force: true });
