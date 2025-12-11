# Pokémon Center API

The **Pokémon Center API** is a backend service inspired by the classic Pokédex from *Pokémon Yellow (1999)*.  
It provides Pokémon data (listing and details) and simulates turn-based battles between Pokémon.

This project follows a layered architecture and is implemented in small, review-friendly milestones.

---

## Endpoints Overview

Below are the API endpoints planned for this project according to the assessment requirements.

---

### **GET api/pokemon — Pokémon List**

Fetches a **paginated list** of Pokémon using the PokéAPI and returns a simplified structure.

**Response includes:**
- `id`
- `name`
- `spriteUrl`

**Query Params:**

| Param   | Default | Description               |
|---------|---------|---------------------------|
| limit   | 20      | Number of Pokémon returned |
| offset  | 0       | Pagination offset         |

---

### **GET api/pokemon/{idOrName} — Pokémon Details**

Fetches **full details** of a Pokémon by ID or name.

**Returns:**
- id  
- name  
- height  
- weight  
- types  
- abilities  
- base stats  
- sprite image URL  


---


### **POST api/battle — Pokémon Battle Simulation**

Executes a **turn-based battle** between two Pokémon.

**Battle rules:**
- Faster Pokémon attacks first  
- Damage is applied turn-by-turn  
- Battle ends when HP ≤ 0  
- The winner is the one with remaining HP  
- Response includes a full battle log  

**Sample Request:**

```json
{
  "pokemon1": "pikachu",
  "pokemon2": "bulbasaur"
}
