#### Sergei Nesterenko

### **Задание:** 
+ **1. Игровая сессия**
+ **1.1.** - игра проходит между несколькими игроками, которые подключились на сервер.
+ **1.2.** - игра длится, пока один из игроков не наберёт необходимое количество очков.
+ **1.3.** - когда один их игроков набирает опредлённое коичество очков, игры отображает имя победителя и через некоторео время перезапускает сессию, сбрасываю прогресс всех игроков.
+ **2. Игрок**
+ **2.1.** - 3d объект, который управляется на WASD.
+ **2.2.** - имеет способность "Рывок". Используется на LMB. При использовании двигает игрока на определённое расстояние и если кто-то из других игроков находился на линии
перемещения, то засчитывает очко игроку совершившим "Рывок". Другой же игрок становится невосприимчивым на определённое время к воздействию на него способностей других игроков
и меняет цвет.

### **Используемые дополнительные технологии:**
  + **1.** - Cinemachine - для более удобного слежения за игроком.
  + **2.** - EventBus - для менее связанного общения разных модулей.
  + **3.** - DOTWeen - для упрощении эффекта рывка.
  
  ### **Реализация:** 
[Короткое видео](https://youtu.be/ajRWZboV-04)

+ **1. Создание игрока:**

![UML](https://user-images.githubusercontent.com/107647367/232340648-050678fe-90fb-43e7-b7db-3a125ef08169.png)

 При подключении игрока на сервер, LoginManager унаследованный от NetworkManager создаёт префаб игрока на сцене и передаёт информацию о нём GameController. GameController в свою очередь, обновляет список игроков у всех клиентов и обновляет информаю на котроллерах отвечающих за отображение информации о других игроках на сцене. После того как игрок заходит в игру, он вводит своё имя в появившемся окне за которое отвечает PlayerNameScreenController (в него прокидывается callback из GameManager, который вызыватся при нажатии кнопки Activate в игре) и начинает игру. Информация о его имени обновляется в классе Player и синхронизируется с информацией на сервере.

+ **2. Управление игроком:**
  Осуществляется в скрипте PlayerManagement, который отвечает за ввод игрока и принятию решения о том, что делать дальше и ничего не знает о других классах. Так на WASD вызывается метод Move (при расширении опций управления можно будет добавить отдельные классы для обработки действий игроком в так NewInputSystem для упрощения маршрутизации и привязки клавишь). А при нажатии на LMB вызывается UnityEvent.

+ **3. Игрок:**

![image](https://user-images.githubusercontent.com/107647367/232341336-c33c76e5-6507-4807-bbef-96d6e1106b6c.png)

  Player хранит в себе ссылки на ScoreController и спиок способностей, которые он может использовать. При поступлении запроса, прокидывает запрос дальше, испольняющему скрипту. Так же, в себе он хранит имя игрока, которое синхронизируется для всех игроков. ScoreController отвечает за счёт конретного игрока и проверяет текущее количество осков и победное количество очков по запросу Player. Победное количество очков настраиваемое на ScoreController. После зачисления очков Player спрашивает у ScoreController, достигнуто ли победное количество очков, если да, то кидает событие в EventBus.

+ **4. Способности:**

  Все способности наследуется от абстрактного класса Skill. При потенциальном добавлении новых способностей их можно наследовать от Skill и прокидывать в [SF] массив Skill в Player. При получении ивента игрок вызывает активацию JerkSkill, внутри которого есть настраиваемые параметры: дистанция рывка, время на которое он меняет состояние другого игрока и цвет, в который он его перекрашивает. При использовании проверяет, коснулся ли он другого игрока, если да, то сообщает об этом Player.

+ **5. Окончание игры и перезапуск:**

![image](https://user-images.githubusercontent.com/107647367/232341884-02694a2b-ad10-4a8b-abb2-1ab0010936a0.png)

  При получении ивента из шины, GameController инициализирует процесс окончания сессии, в котором отключает игроков, сбрасывает их данные, включает для всех WinScreen и говорит PlayerRespawnController начать процесс перезагрузки игроков. Когда время перезагрузки прийдёт (время настраивается в PlayerRespawnController), он вновь включит объекты игроков и игра начнётся заного.
