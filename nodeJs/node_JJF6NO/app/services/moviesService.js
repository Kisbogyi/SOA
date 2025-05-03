// Import express:
// import express, {Request, Response} from 'express';
const express = require('express');
const bodyParser = require('body-parser');
const jsonParser = bodyParser.json();


// Create a new express router:
const router = express.Router();

const Movie = require('../schemas/movie');
const createError = require('http-errors');

// interface RequestParams {}
// interface ResponseBody {}
// interface RequestBody {}
// // Specify that 'name' is a query parameter of type 'string':
// interface RequestQuery {
// 	name;
// }

/* GET request: */
router.get('/', async (req, res, next) => {
	// Get the 'name' query param:
	const movies = await Movie.find({}, '-_id title year director actor');
	if (movies) {
		// Send response:
		res.send(JSON.stringify({movie: movies}));
	} else {
		return next(createError.NotFound())
	}
}
);

router.post('/', jsonParser, async (req, res, next) => {
		req.body._id = Math.floor(Math.random() * Number.MAX_SAFE_INTEGER);
		const movie = new Movie(req.body);
		await movie.save();
		res.send(JSON.stringify({id: movie._id}));	
	}
)

router.get('/find', async (req, res, next) => {
	const year = req.query.year;	
	const orderBy = req.query.orderby;	
	if (orderBy === "Title"){
		const ids = await Movie.find({year: year}, '_id').sort({title: 1});
		res.send(JSON.stringify({id: ids.map((collection) => collection._id)}));
		
	}
	else {
		const ids = await Movie.find({year: year}, '_id').sort({director: 1});
		res.send(JSON.stringify({id: ids.map((collection) => collection._id)}));
	}
});

router.get('/:id', async (req, res, next) => {
	// Get the 'name' query param:
	const id = req.params.id;
	const movies = await Movie.findOne({_id: id}, '-_id title year director actor');
	if (movies) {
		// Send response:
		res.send(JSON.stringify(movies));
	} else {
		return next(createError.NotFound())
	}
}
);

router.put('/:id', jsonParser, async (req, res, next) => {
	const id = Number(req.params.id);
	const out = await Movie.findOneAndUpdate({_id: id}, req.body, {new: true });
	if (!out) {
		req.body._id = id;
		await new Movie(req.body).save();
	}
	res.send();
	}
)

router.delete('/:id', async (req, res, next) => {
	const id = Number(req.params.id);
	await Movie.deleteOne({_id: id}, req.body);
	res.send();
});



// Export the router:
module.exports = router;
