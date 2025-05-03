// import express, { Application, Request, Response } from 'express';
const express = require('express');
const app = express();

const PORT = 3000;

// import bodyParser from 'body-parser';
const bodyParser = require('body-parser');
app.use(bodyParser.json());

const mongoose = require('mongoose');

const moviesService = require('./services/moviesService.js');

// Routes
app.use("/movies", moviesService);


// Mongodb init
console.log("start to connect to mongodb");
mongoose.connect('mongodb://127.0.0.1:27017/JJF6NO')
	.then(res => {
		console.log("Connected to MongoDB");
		app.listen(PORT, () => {
			console.log('Listening on:', PORT);
		});
	})
	.catch(err => console.log(err));
