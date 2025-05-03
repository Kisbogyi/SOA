const mongoose = require('mongoose');

const MovieSchema = new mongoose.Schema({
	title: String,
	year: Number,
	director: String,
	actor: [String],
	_id: Number
});

const Movie = mongoose.model('Movies', MovieSchema);
module.exports = Movie;
