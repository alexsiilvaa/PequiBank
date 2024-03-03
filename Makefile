build:
	docker-compose build

infra:
	docker-compose up -d db

run: infra build
	docker-compose up -d nginx

stop:
	docker-compose stop

delete:
	docker-compose rm -s -f