Start with Docker:
```
docker build . -t mixter-php
docker run -it --rm -p 8000:8000 -v ${PWD}:/app mixter-php
```

You should be logged in Docker
```
cd /app
composer install
```

Run tests: `vendor/bin/phpunit`

Launch Laravel dev server: `php artisan serve --host=0.0.0.0`

Test HTTP server using routes defined in `app/Http/routes.php` using `curl` for example
