# Pokémon Center API

The Pokémon Center API is a backend service inspired by the classic Pokédex from Pokémon Yellow (1999).

It provides Pokémon data (listing and details) and supports turn-based battle simulations between Pokémon, using simplified and deterministic rules.

The project follows a layered architecture and is implemented in small, review-friendly milestones.

[Pokémon Center API Technical Assessment](https://docs.google.com/document/d/1y01YcWOn1Y3gXJyRrh1lChvf924p0bckHfa0i2SGg94/edit?usp=sharing)

---

## Endpoints Overview

Below are the available API endpoints according to the assessment requirements.

---

## GET /api/pokemon — Pokémon List

Fetches a paginated list of Pokémon using the PokéAPI and returns a simplified structure.

Response includes:
- id
- name
- spriteUrl

Query Params:

- limit (default: 20) — Number of Pokémon returned
- offset (default: 0) — Pagination offset

---

## GET /api/pokemon/{idOrName} — Pokémon Details

Fetches full details of a Pokémon by ID or name.

Returns:
- id
- name
- height
- weight
- types
- abilities
- base stats
- sprites

---

## POST /api/battle/simulate — Pokémon Battle Simulation

Executes a turn-based battle between two Pokémon.

Battle rules:
- Turn-based combat
- Pokémon attack order is defined by Speed
- If both Pokémon have the same Speed, the first Pokémon in the request starts
- Damage is calculated as attack - defense (minimum damage is 1)
- HP is reduced turn by turn
- The battle never ends in a tie
- The response includes a full battle log

Sample request body:

~~~json
{
  "firstPokemon": "pikachu",
  "secondPokemon": "bulbasaur"
}
~~~


Sample response (simplified):

~~~json
{
  "winner": {
    "id": 1,
    "name": "bulbasaur"
  },
  "loser": {
    "id": 25,
    "name": "pikachu"
  },
  "totalTurns": 8,
  "turns": [
    {
      "turn": 1,
      "attacker": { "id": 25, "name": "pikachu" },
      "defender": { "id": 1, "name": "bulbasaur" },
      "damage": 6,
      "defenderHpAfter": 39
    }
  ]
}
~~~


---

## Architecture Notes

- The battle engine (domain) is pure and independent from HTTP and external APIs
- Pokémon IDs are resolved only at the application layer
- External communication with the PokéAPI is isolated
- The API favors clarity and simplicity over overengineering
