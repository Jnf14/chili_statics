'use strict';
const child_process = require('child_process');
const fs = require('fs');
const express = require('express');
const cors = require('cors');
const path = require( "path" );
let mysql = require('mysql'); // Import mysql node module

const alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

// Define process path
process.chdir("/Users/jeremiefrei/Documents/EPFL_local/Bachelor\ Project/chili_statics/statics-node-server");
const directory = ".";

// Build connection to MySQL database
let connection = mysql.createConnection({
    host: 'localhost',
    user: 'root',
    password: '12345678',
    database: 'statics'
});

// Connect to MySQL
connection.connect(function(err) {
  if (err) return console.error('error: ' + err.message);
});

// Frame3dd app
const f3dd_port = 1337;
const f3dd_app = express();
f3dd_app.use(cors());

// MySQL app
const mysql_port = 1338
const mysql_app = express();
mysql_app.use(cors());

// F3DD GET
f3dd_app.get('/', function (req, res) {
    res.writeHead(200, { 'Content-Type': 'text/plain' });
    res.end('Hello World\n');
});

// F3DD PUT
f3dd_app.put('/', function (req, res) {
    req.on('data', function (data) {
        let fileName = "";
        for (let i = 0; i < 10; i++) {
            fileName += alphabet.charAt(Math.floor(Math.random() * alphabet.length));
        }
        fileName += Date.now().toString();
        fs.writeFile(directory+"/"+fileName + '.3dd', data, function (err) {
            if (err) {
                return console.log(err);
            }
            child_process.execFile('./osx/frame3dd',
                [directory +'/'+ fileName + '.3dd', directory + '/' +fileName + '.out'],
                (error, stdout, stderr) => {
                    //console.log(error);
                    //console.log(stdout);
                    //console.log(stderr);
                    fs.readFile(directory + "/"+fileName + '.out', (err, data) => {
                        if (err) {
                            return console.log(err);
                        }
                        fs.readFile('/tmp/' + fileName + '-mshf.001', (err, data1) => {
                            if (err) {
                                return console.log(err);
                            }
                            res.end(data.toString() + data1.toString());
                            //console.log(data.toString() + data1.toString());

                            fs.unlink(directory+'/'+ fileName + '.3dd', (err) => {
                                if (err) {
                                    return console.log(err);
                                }
                            });
                            fs.unlink(directory+ '/'+fileName + '.if01', (err) => {
                                if (err) {
                                    return console.log(err);
                                }
                            });
                            fs.unlink(directory+'/' + fileName + '.plt', (err) => {
                                if (err) {
                                    return console.log(err);
                                }
                            });
                            fs.unlink(directory+'/' + fileName + '.out', (err) => {
                                if (err) {
                                    return console.log(err);
                                }
                            });
                            fs.unlink('/tmp/' + fileName + '-msh', (err) => {
                                if (err) {
                                    return console.log(err);
                                }
                            });
                            fs.unlink('/tmp/' + fileName + '-mshf.001', (err) => {
                                if (err) {
                                    return console.log(err);
                                }
                            });
                            fs.unlink('./_out.CSV', (err) => {
                                if (err) {
                                    return console.log(err);
                                }
                            });
                        })
                    });

                });
        });
    });
});

mysql_app.get('/', function (req, res) {
    res.writeHead(200, { 'Content-Type': 'text/plain' });
    res.end('Hello World\n');
});

// MySQL PUT
mysql_app.put('/', function (req, res) {
  req.on('data', function (data){
      // Parse given data to json
      var json = JSON.parse(data.toString());

      // Query to insert data
      var sql = "INSERT INTO Log (objectType, action, details) VALUES (?,?,?)";
      var vals = [json.objectType, json.action, json.details];
      connection.query(sql, vals, (err, results, fields) => {
        if (err)
          return console.error('error: ' + err.message);
        // else
        //   console.log("added log")
      });
  });
});

// f3dd_app listens to port f3dd_port
f3dd_app.listen(f3dd_port, () => console.log(`Frame3DD Server started on port ${f3dd_port}`));
mysql_app.listen(mysql_port, () => console.log(`MySQL Server started on port ${mysql_port}`));
