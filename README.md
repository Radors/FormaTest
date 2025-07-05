# FormaTest

FormaTest is an API for generating test data in the the form of JSON trees with random structure.

## Usage

Endpoint `/generate` expects 3 query parameters: `count`, `depth`, `value`.

The API is live at `formatest.enrau.com/generate`

`formatest.enrau.com/generate?count=100&depth=20&value=arbitrary`

* `count` is total number of nodes.
* `depth` is maximum depth.
* `value` is an arbitrary string value to include once in the tree.

Count and depth are excluding the value node from their totals.

If you prefer to run it on your own machine:

`docker pull radddan/formatest:latest`

`docker run radddan/formatest:latest`

## Tech

* C#
* ASP.NET Core Web API (.NET 10-Preview)
* Native AOT
* Docker
* Azure


