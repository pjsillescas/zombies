# prog3D-PEC Final

Como esta práctica es una continuación de la PEC3, la documentación de la misma se deja debajo para poder consultar la implementación de sus puntos.

## Controles
- Movimiento: W A S D
- Salto: espacio
- Disparo primario: click izquierdo del ratón
- Disparo secundario: Click derecho del ratón
- Cambio de arma: rueda del ratón
- Sprint: Shift + movimiento
- Interactuar con coche (entrar y salir): Tecla F

Control implementado con el New Input System.

## Recursos

- Build descargable (Windows): https://periquillo.itch.io/juegos3d-pecfinal-2023
- Video: https://youtu.be/IwAYeCeAz-U

En la página de itch.io también existe una versión webgl.

## Instalación del juego

Windows: Descargar Windows.zip y descomprimir en cualquier carpeta. Para arrancar el juego, simplemente hay que ejecutar PEC3.exe.
Webgl: Pulsar el botón Run Game directamente en la página del juego en itch.io.

## Puntos Básicos

### 1. Menú para iniciar partida y menú de opciones.

El menú inicial de la PEC3 se ha ampliado con sendos botones para lanzar el menú de opciones y para jugar al nuevo nivel de la PEC final (manteniendo el nivel de la PEC3). El menú de opciones consta de dos opciones:
- Idioma: español o inglés,  se traducen todos los textos del juego via un servicio de traducciones basado en un fichero json almacenado en Resources.
- Modo gore: Si está activo, los zombies y peatones lanzarán sangre roja al ser golpeados, siendo verde en caso contrario.


### 2. Tener una ciudad con edificios y vegetación llena de zombies.

Este mapa está basado en la estructura del mapa de la PEC3, pero con una red de carreteras un poco más complejo para hacer interesante la circulación de los coches.


### 3. Los zombies tendrán una IA similar a los drones, pero en vez de patrullar se irán moviendo a puntos random cerca de ellos. Y si te ven, irán corriendo directamente a por el player.

Punto implementado en su totalidad en la PEC3.


### 4. Peatones caminando y coches circulando de manera autónoma por la ciudad.

#### Peatones

Estos agentes (prefab *Pedestrian*) tienen una estructura muy similar a los zombies, incluso en su IA, siendo un híbrido entre los zombies de la PEC3 y los enemigos de la PEC2. Siguiendo un paralelismo con los zombies, su IA, controlada por el componente *PedestrianAIManager*, consta de dos estados:
- Viaje (*TravelState*): En este estado, se elige uno de los lugares disponibles en la escena, y el peatón intentará ir desde su posición actual hasta ese lugar elegido usando el navmesh de la escena. Para dar más realismo a estos agentes, el navmesh consta de dos tipos de areas de navegación: el área por defecto *Walkable* y las carreteras *Road*, que para los peatones tienen un coste de 500 para evitar que sean víctimas potenciales de los coches.
- Huida (*RunawayState*): Este estado se activa cuando algún zombie entra en su campo de visión, reutilizando el componente *Radar* de los zombies que responde a objetos con el tag "Zombie". Todos estos zombies visibles se almacenan en una lista, que será usada en cada *Update*, donde el peatón elegirá la dirección contraria a la resultante de sumar los vectores de desplazamientos de estos zombies con respecto al peatón, con el objetivo de alejarse de ellos.


#### Coches

Los coches se han implementado en el prefab *CarStruct*. Al ser reciclados de los Starter Assets de Unity, y estando el proyecto con el New Input System y renderizado URP, ha habido que actualizar sus materiales y sus scripts para que funcionen en estas condiciones.

La IA de los coches (implementada en el componente *CarAIManager*) se realiza buscando un camino en el grafo de la carretera de la escena, donde se han marcado puntos de referencia que se han unido en forma de grafo dirigido. Esta estructura de grafo dirigido permite implementar de forma fácil los carriles de distinto sentido en las carreteras. Para el movimiento de los coches se busca un nodo objetivo en el grafo y se busca un camino vía algoritmo de búsqueda primero en profundidad, todo en el componente *RoadMap*. Como el objetivo de los coches es simplemente circular no se busca un camino mínimo, sino cualquiera que conduzca al nodo objetivo. Y para ir de un nodo a otro, los coches usan el navmesh restringiendo su navegación al área *Road*.


### 5. Los peatones deberán huir de los zombies.

Comportamiento explicado en el apartado anterior.


### 6. Como en todo buen GTA, deberemos poder robar un coche y circular con él.

Para implementar este punto, se ha usado una interfaz *IInteractable* con la que el jugador podrá interactuar con elementos de la escena de forma abstracta mediante el script *InteractionManager*, que reacciona cuando colisiona con un objeto que implementa *IInteractable*. Los coches usan la implementación *CarRideInteract* en un volumen dentro del cual el jugador podrá entrar al coche pulsando la tecla F. De la misma manera, el jugador podrá bajarse del coche pulsando F de nuevo.

Una vez que el jugador ha tomado control del coche, la IA se desactiva y no vuelve a activarse, considerándose el coche abandonado. El jugador siempre puede volver a conducir un coche al que dejó abandonado anteriormente.


## Puntos optativos

### 1. Semáforos que controlan automáticamente el tráfico cada cierto tiempo.

El sistema de tráfico se compone de los semáforos, formados por los scripts *Semaphore* y *SemaphoreController*, y un box collider situado en la parte delantera de los coches por donde percibirán los semáforos y otros coches para pararse o seguir su trayecto.

Los semáforos tienen tres estados *SemaphoreState.red*, *SemaphoreState.amber* y *SemaphoreState.green*, pasando de rojo a verde directamente y parando un momento en amarillo al pasar de verde a rojo como en un semáforo normal. Los coches se pararán al detectar un semáforo en estado rojo, o un coche, simulando colas de tráfico. Al volverse el semáforo verde o dejar de percibir al coche de delante, éstos proseguirán su marcha.


### 2. Si los peatones son atrapados por un zombie, que se transformen en zombie también.

Para implementar este punto, se ha añadido al *DamageableComponent* el enumerado *DamageType* que modela el tipo de daño recibido. En este caso, si el peatón muere recibiendo un daño de tipo *DamageType.zombie*, al morir se instanciará en su posición un zombie ordinario.


### 3. Esconder diferentes armas por el escenario e implementar su funcionalidad.

En el mapa están situados dos ítems de armas: la escopeta *Shotgun*, implementada en la PEC3, y el zombificador, una arma con una cadencia muy lenta y muy débil que ejerce 6 puntos de daño de tipo *DamageType.zombie*, lo suficiente para matar a un peatón de un solo golpe y forzar su transformación en zombie.


### 4. Si atropellamos a los zombies o a los petaones, estos deberían explotar.

De la misma forma que los peatones instancian un zombie al morir de un ataque de tipo zombie, cuando lo hacen por un atropello (los coches hacen un daño de tipo *DamageType.hit* de 1000 puntos de vida, lo que equivale a muerte directa en todos los personajes del juego), instancian un personaje cuya malla está partida en varios trozos que son sometidos a una fuerza de explosión simulando el golpe recibido.


### 5. ~~Sonorizar a los zombies, humanos, coches, y nuestro personaje.~~

No implementado.


## ------------------------------------------------------------------------------------------------------------------------------

# prog3D-PEC3

## Controles
- Movimiento: W A S D
- Salto: espacio
- Disparo primario: click izquierdo del ratón
- Disparo secundario: Click derecho del ratón
- Cambio de arma: rueda del ratón
- Sprint: Shift + movimiento

Control implementado con el New Input System.

## Recursos

- Build descargable (Windows): https://periquillo.itch.io/juegos3d-pec3-2023
- Video: https://youtu.be/BkqHI04MhmY

En la página de itch.io también existe una versión webgl, que si bien va un poco lento, se puede jugar.

## Instalación del juego

Windows: Descargar Windows.zip y descomprimir en cualquier carpeta. Para arrancar el juego, simplemente hay que ejecutar PEC3.exe.
Webgl: Pulsar el botón Run Game directamente en la página del juego en itch.io.

## Puntos Básicos

### 1. Crear un pequeño pueblecito con los assets disponibles en el ejemplo de los materiales u otros si queréis.

El escenario es un pequeño barrio postapocalíptico sitiado por el ejército. Los assets que se han usado son de los paquetes siguientes:
- Toon Gas Station para las carreteras y edificios
- Sci-fi Lite para la base que sitia el barrio
- First Aid Kit para los ítems
- Soldier Sounds Pack para los sonidos de las armas y de los ítems
- Playground Low Poly para los assets del parque
- Synty Polygon Prototype para los personajes de los zombies, jugador y las armas
- Personaje Maynard de Mixamo para el jefe


### 2. El personaje deberá tener una pistola para disparar en línea recta hacia adeltante siempre (no hace falta apuntar).

La arquitectura de las armas se ha implementado de la misma forma que en la PEC2, reutilizando la arquitectura y los scripts casi en su totalidad, respetando la lista de armas en el apartado primario y reservando el arma secundaria para la espada. Se sigue usando el script *Weapon* para implementar las armas de fuego, pudiendo realizar disparos en línea recta via raycast con un cooldown. De forma adicional, si el jugador deja pulsado el botón de disparo, su personaje disparará automáticamente una vez termine el cooldown de su arma.

La gestión de las armas se sigue haciendo mediante el script *ShooterComponent*, que mantiene la lista de armas primarias disponibles y el arma secundaria, permitiendo equiparlas y usarlas cuando sea preciso. También siguen disponibles todos los eventos generados por *ShooterComponent* (*OnAnyWeaponSelected* para informar al equipar un arma, *OnAnyWeaponShot* para informar de un disparo con un arma y la munición gastada) para tener actualizados los widgets de la interfaz de usuario y/o cualquier otro elemento del gameplay que necesite dicha información.

Las armas disponibles en esta PEC son las siguientes:
- Rifle: arma primaria principal equipada en el personaje
- Espada: arma secundaria, cuerpo a cuerpo
- Escopeta: arma primaria para dañar a múltiples objetivos cercanos, equipable al recoger su ítem en el nivel

Las tres armas tienen lógicas distintas, implementadas en sus respectivos scripts, todos heredando del arma principal *Weapon*:
- El rifle usa la funcionalidad principal del script *Weapon*, comentado anteriormente
- La espada activa y desactiva la posibilidad de dañar todos los *DamageableComponent*s a su alcance mediante __animation events__, situados en la animación *SwordSlash*
- La escopeta tiene situado un collider a su alrededor, y al disparar, todos los *DamageableComponent*s dentro del mismo serán dañados con el máximo de daño por simplicidad, pero podría añadirse una lógica de daño inversamente proporcional a la distancia


### 3. El personaje deberá estar completamente animado. Idle, caminar, correr, saltar, girar, disparar, recibir impacto y morir.

Todas las animaciones han sido obtenidas de Mixamo, y configuradas en el controlador PlayerAnimController.

### 4. Tanto la cantidad de vida como la munición deberán siempre verse por pantalla (HUD).

Para este apartado también se han reutilizado los componentes del HUD de la PEC2 *HealthUI* y *WeaponryUI* para mostrar la vida y la información del arma equipada. Y de la misma manera que para las armas se ha reutilizado el script *ShooterComponent*, para la salud se ha reutilizado también el script *DamageableComponent*, aunque retirando la gestión del escudo que sí se usaba en la PEC2.

### 5. Los enemigos serán zombies que irán paseando por la ciudad y cuando el player esté cerca irán siempre caminando hacia el personaje y lo atacarán cuando esté a su lado.

La inteligencia artificial de los zombies es muy similar a la que tenían los enemigos de la PEC2, basada en una máquina de estados, pero debido al ataque cuerpo a cuerpo de éstos, hay algunas diferencias. Los estados son los siguientes:
- *RoamState*: En este estado de relajación, los zombies vagarán de forma errática por el nivel, escogiendo un punto aleatorio cercano al agente.
- *AlertState*: Una vez que el jugador entra en el campo de percepción del agente, el zombie irá a perseguirlo mientras que siga percibiéndolo.
- *AttackState*: Este estado se activa si el jugador está a una distancia cercana al zombie. Los ataques van a consistir en guantazos cuyo daño viene controlado mediante animation events, de forma similar a la espada del jugador.


### 6. Los enemigos deberán tener animación de caminar, atacar, morir y recibir impacto.

Al igual que en el caso del jugador, todas las animaciones de los zombies han sido obtenidas de Mixamo, y se usan en el animator *ZombieController*. También hay otro juego de animaciones similares para usarlas con el boss del nivel, que es un zombie más grande con otra skin para diferenciarlo más de los zombies ordinarios. Este boss usa el animator *BossAnimationController*.


### 7. Poner sistemas de partículas cuando te hieren o cuando un zombie es golpeado o muere.

Estos sistemas de partículas se activan en los eventos *OnDamage* y *OnHitPointsDepleted* de los componentes *DamageableComponent* de los zombies y el jugador.


### 8. Repartidos por el escenario deberán haber ítems de ‘vida’ y ‘munición’.

Esta PEC se ha realizado con el punto optativo 9 en mente, así que la generación tanto de zombies como de ítems se hace de forma automática cada cierto tiempo configurable en ciertos lugares del nivel. Por defecto se crea un nuevo item cada minuto.  

### 9. El juego deberá tener pantalla de game over y que al morir podamos reiniciar el nivel.

Este elemento también se ha reutilizado de la PEC2, activado por el evento *OnHitPointsDepleted* del jugador. En este menú se puede elegir empezar la partida de nuevo o ir al menú principal.

Este menú, además de informarnos si hemos muerto o hemos escapado, nos muestra el tiempo que hemos durado en el juego. Este tiempo, que también puede verse en la parte superior del HUD durante el juego, es calculado por el script TimerUI, que también dispara un evento público *OnTimerTick* cada intervalo de tiempo *TimerTickSeconds*, usado por el script *ZombieSpawnerManager* para añadir zombies de manera automática al juego.


## Puntos optativos

### 1. Crear situaciones donde sea necesario saltar para llegar a ciertas zonas.

En el nivel hay dos situaciones donde el salto es necesario:
- En la explanada central hay una serie de plataformas con el ítem de la escopeta, situado en la segunda plataforma superior, y en la primera hay un item con munición para la misma,
- Una vez se ha conseguido entrar en el parque con la llave del boss, hay posibilidad de salir de ahí sin pasar de nuevo por los pasillos usando otro sistema de plataformas para subir a los techos de los pasillos, teniendo una vista más alta del nivel. 


### 2. Añadir nuevas armas como granadas o una escopeta.

Se ha implementado la escopeta, como se comentó en el punto obligatorio 2.


### 3. ~~El juego deberá estar sonorizado completamente.~~

No implementado completamente. Solamente hay sonidos en los ítems, los disparos y los pasos del jugador.


### 4. Hacer diferentes tipos de zombies. Más fuertes, resistentes o rápidos.

Para cada zombie se puede configurar su salud (*HealthPoints*), velocidades de andar y correr (*WalkSpeed* y *RunSpeed*) y su daño (*Damage*). Por simplicidad, los scripts *ZombieSpawner* solo generan zombies con la misma configuración, a diferencia del boss, que es una copia de la estructura lógica de un zombie ordinario con más puntos de vida y más capacidad de daño.


### 5. ~~Crear la posibilidad de poder apuntar manualmente al disparar.~~

No implementado.


### 6. Crear algún tipo de puzzle donde sea necesario encontrar una llave para abrir una nueva zona. Esa llave la puede dejar algún zombie ‘boss’ al morir.

Para entrar al parque del sur hace falta que el jugador obtenga la llave de su puerta matando al zombie boss que vaga por el nivel. El sistema de puertas y llaves también ha sido reutilizado de la PEC2, tanto la lógica como la interfaz de usuario.

### 7. Que el player tenga otro arma, un cuchillo o espada para matar a los zombies que tenga justo enfrente.

Se ha implementado la espada como arma secundaria del jugador, explicado en el punto obligatorio 2.


### 8. Los enemigos al morir podrían dejar items en el suelo.

También se reutiliza el script *PickupGeneratorComponent* para que los zombies generen ítems al morir, tanto de salud como munición o llaves, en el caso del boss.


### 9. Los zombies irán ‘spawneando’ por la ciudad de manera random o en puntos concretos a medida que el player los va matando o cada cierto tiempo prefijado. A más rato llevemos jugando más rápido han de aparecer. La idea es hacer un survival que cada vez sea más difícil sobrevivir.

Como se ha adelantado anteriormente, todos los zombies se crean automáticamente en distintos puntos de la escena cada cierto intervalo de tiempo. Este proceso lo hace el script *ZombieSpawner* controlado por el script *ZombieSpawnerManager*, que tiene como misión elegir qué zombie spawner creará el siguiente zombie. Para ello tiene varias políticas de elección de creador de zombies:
- Aleatorio: se elige un spawner aleatorio entre todos los disponibles
- Más cercano: se elige el spawner más cercano al jugador
- Más lejano: al contrario que el anterior, se elige el spawner situado a la mayor distancia del jugador
- Cercano: Variación de la política del más cercano, que consiste en elegir de la lista de spawners a alguno de los vecinos del spawner más cercano para crear el zombie, incluído el mismo spawner más cercano. Esto hace que si el jugador no se mueve mucho, no se creen todos los zombies en el mismo sitio siempre, pudiendo jugar también con la posición de los spawners en el nivel

Para evitar un desbordamiento de memoria, se ha limitado el número de zombies en juego a 200, valor configurable en el editor.

### Puntos extra

- La implementación del punto de extracción del pueblo se ha hecho reutilizando el script *FinishLevel* realizado para la PEC2.