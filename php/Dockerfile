FROM php:8.2-apache

# Install system dependencies for PostgreSQL
RUN apt-get update && apt-get install -y \
    libpq-dev \
    && docker-php-ext-install pdo pdo_pgsql \
    && rm -rf /var/lib/apt/lists/*

# Enable Apache mod_rewrite (optional)
RUN a2enmod rewrite

# Copy source code
COPY . /var/www/html/

