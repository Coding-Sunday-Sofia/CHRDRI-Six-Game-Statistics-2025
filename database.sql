--
-- PostgreSQL database dump
--

-- Dumped from database version 15.14 (Debian 15.14-1.pgdg13+1)
-- Dumped by pg_dump version 15.13

-- Started on 2025-08-25 10:34:57 UTC

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 4 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: pg_database_owner
--

CREATE SCHEMA public;


ALTER SCHEMA public OWNER TO pg_database_owner;

--
-- TOC entry 3413 (class 0 OID 0)
-- Dependencies: 4
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: pg_database_owner
--

COMMENT ON SCHEMA public IS 'standard public schema';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 215 (class 1259 OID 24578)
-- Name: statistics; Type: TABLE; Schema: public; Owner: myuser
--

CREATE TABLE public.statistics (
    id integer NOT NULL,
    gguid character(255) NOT NULL,
    turn integer NOT NULL,
    board text NOT NULL
);


ALTER TABLE public.statistics OWNER TO myuser;

--
-- TOC entry 214 (class 1259 OID 24577)
-- Name: statistics_id_seq; Type: SEQUENCE; Schema: public; Owner: myuser
--

CREATE SEQUENCE public.statistics_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.statistics_id_seq OWNER TO myuser;

--
-- TOC entry 3414 (class 0 OID 0)
-- Dependencies: 214
-- Name: statistics_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: myuser
--

ALTER SEQUENCE public.statistics_id_seq OWNED BY public.statistics.id;


--
-- TOC entry 3263 (class 2604 OID 24581)
-- Name: statistics id; Type: DEFAULT; Schema: public; Owner: myuser
--

ALTER TABLE ONLY public.statistics ALTER COLUMN id SET DEFAULT nextval('public.statistics_id_seq'::regclass);


--
-- TOC entry 3265 (class 2606 OID 24585)
-- Name: statistics statistics_pkey; Type: CONSTRAINT; Schema: public; Owner: myuser
--

ALTER TABLE ONLY public.statistics
    ADD CONSTRAINT statistics_pkey PRIMARY KEY (id);


-- Completed on 2025-08-25 10:34:57 UTC

--
-- PostgreSQL database dump complete
--

