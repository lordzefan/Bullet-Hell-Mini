# Bullet Hell Mini

Top-down bullet hell shooter dimana player harus bertahan hidup dari serangan enemy dan menyelesaikan seluruh wave hingga boss terakhir.

---

# Gameplay

Player bergerak bebas di arena sambil:
- menghindari projectile musuh,
- menyerang enemy dan mengalakan enemy,
- bertahan hidup hingga seluruh wave selesai.

Game berfokus pada:
- performa ketika banyak projectile aktif,
- object pooling,
- enemy attack pattern,
- gameplay bullet hell sederhana.

---

# Controls

| Action | Key |
|---|---|
| Move | WASD |
| Shoot | Left Mouse Button |

---

# UI Navigation

| UI | Function |
|---|---|
| Health UI | Menampilkan HP player |
| Wave UI | Menampilkan wave yang sedang dimulai |
| Game Over UI | Muncul saat player mati |
| Win UI | Muncul saat seluruh wave selesai |

---

# Win Condition

Player berhasil mengalahkan:
- seluruh enemy wave,
- boss pada wave terakhir.

---

# Lose Condition

Player kalah ketika:
- HP player mencapai 0.

---

# Features Implemented

## Core Features
- 8-direction player movement.
- Shooting mechanic dengan configurable fire rate.
- Health system dengan damage feedback.
- Restart system tanpa restart aplikasi.
- Wave system.

---

## Enemy Types

### Chaser Enemy
Enemy mengejar player secara langsung.

### Shooter Enemy
Enemy berpindah posisi lalu menembak projectile spread.

### Charger Shoot Enemy
Enemy bergerak lurus ke arah player sambil menembak.

### Boss Enemy
Boss memiliki 2 phase:
- Phase 1: Minigun attack ke arah player.
- Phase 2: Radial bullet pattern, menembak kesegala arah.

---

## Performance Features
- Object Pooling untuk projectile.
- Projectile reuse system.
- Separate projectile pool management.
- ScriptableObject untuk projectile data dan jenis projectile.

---

## Projectile Features
Projectile memiliki:
- configurable speed,
- configurable lifetime,
- configurable damage,
menggunakan ScriptableObject.

---

## Save & Load System
Game memiliki sistem save dan load sederhana menggunakan JSON serialization.

Fitur:
- Menyimpan highscore secara otomatis.
- Load highscore saat game dijalankan kembali.
- Menggunakan `JsonUtility` Unity.


---


# Features Not Implemented

## Enemy Pooling
Enemy masih menggunakan Instantiate() dan belum menggunakan object pooling penuh.

### Reason
Waktu pengerjaan lebih difokuskan pada optimisasi projectile karena projectile merupakan bottleneck utama pada bullet hell game.

---

## Advanced Boss Pattern
Boss belum memiliki:
- laser attack,
- summon mechanic,
- multi-pattern randomization.

### Reason
Scope waktu pengerjaan terbatas.

---

## Audio System
Game belum memiliki:
- sound effect,
- background music.

### Reason
Prioritas utama pada gameplay dan performance.

---

# Reflection

## Known Bugs / Limitations

- Projectile radial boss tidak mengikuti rotasi bos jadi projectile muncul diarah yang sama
- Beberapa projectile rotation masih sensitif terhadap transform hierarchy.
- Enemy belum menggunakan pooling sehingga performa masih bisa ditingkatkan.
- Terkadang player tidak terkena damage collision pertama;

---

## Things To Improve

Jika ada waktu tambahan:
- implementasi full enemy pooling,
- menambah variasi bullet pattern,
- menambahkan audio dan visual effect,
- menambahkan pause menu,
- menambahkan difficulty scaling,
- menambahkan animation dan polish.

---

## Biggest Challenges

Tantangan terbesar selama pengerjaan:
- menjaga performa ketika ratusan projectile aktif,
- mengatur object pooling,
- menghindari bug rotation projectile,
- membuat boss pattern tetap stabil.

---

# AI asisten 

Ai yang digunakan yaitu chatgpt sebagai :
- assistant untuk brainstorming gameplay,
- membantu debugging,
- membantu menemukan bug,
- membantu dokumentasi dan README.

---

# Technical Notes

Game dibuat menggunakan:
- Unity Engine
- C#
- Object Pooling
- ScriptableObject Architecture

---

# Credits

Developed for technical test / assignment purpose.
Asset sprite :
- player spaceship = https://www.hiclipart.com/free-transparent-background-png-clipart-zfojn
- enemy chaser spaceship = https://www.hiclipart.com/free-transparent-background-png-clipart-dgcnw
- enemy shooter spaceship = https://www.hiclipart.com/free-transparent-background-png-clipart-dswvq
- enemy charger shooter spaceship = https://www.hiclipart.com/free-transparent-background-png-clipart-iesua
- space background = https://opengameart.org/content/space-background-7
- projectile = https://opengameart.org/content/lasers-and-beams 

---

