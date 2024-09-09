BEGIN;

DO
$$
    BEGIN
        CREATE DATABASE keycloak;
        CREATE USER keycloak WITH ENCRYPTED PASSWORD '<your_password>';
        GRANT ALL PRIVILEGES ON DATABASE keycloak TO keycloak;
        EXECUTE 'GRANT ALL ON SCHEMA public TO keycloak' USING 'keycloak';

        CREATE DATABASE expenso;
        CREATE USER expenso WITH ENCRYPTED PASSWORD '<your_password>';
        GRANT ALL PRIVILEGES ON DATABASE expenso TO expenso;
        EXECUTE 'GRANT ALL ON SCHEMA public TO expenso' USING 'expenso';

        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;
    END
$$;
