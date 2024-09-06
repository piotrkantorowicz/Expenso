CREATE DATABASE keycloak;
CREATE USER keycloak WITH ENCRYPTED PASSWORD 'Keycloak321!';
GRANT ALL PRIVILEGES ON DATABASE keycloak TO keycloak;
\c keycloak postgres
GRANT ALL ON SCHEMA public TO keycloak;

CREATE DATABASE expenso;
CREATE USER expenso WITH ENCRYPTED PASSWORD 'Expenso321!';
GRANT ALL PRIVILEGES ON DATABASE expenso TO expenso;
\c expenso postgres
GRANT ALL ON SCHEMA public TO expenso;