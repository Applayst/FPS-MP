import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";


export class Player extends Schema {

    @type ("boolean")
    team = true;

    @type("number")
    speed = 0;

    @type("int8")
    maxHP = 0;

    @type("int8")
    currentHP = 0;

    @type("number")
    pX = Math.floor(Math.random() * 50) - 25;

    @type("number")
    pY = 0;

    @type("number")
    pZ = Math.floor(Math.random() * 50) - 25;

    @type("number")
    vX = 0;

    @type("number")
    vY = 0;

    @type("number")
    vZ = 0;

    @type("number")
    rX = 0;

    @type("number")
    rY = 0;

    @type("boolean")
    s = false;

    @type("number")
    sitM = 0;

    @type("int8")
    iGun = 0;
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    @type("uint8")
    lossA = 0;

    @type("uint8")
    lossB = 0;

    something = "This attribute won't be sent to the client-side";

    createPlayer(sessionId: string, data: any) {
        const player = new Player();
        player.speed = data.speed;
        player.maxHP = data.hp;
        player.currentHP = data.hp;
        player.sitM = data.sitM;

        player.team = ((this.players.size % 2) != 0);

        this.players.set(sessionId, player);
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    movePlayer (sessionId: string, data: any) {
        const player = this.players.get(sessionId);
        player.pX = data.pX;
        player.pY = data.pY;
        player.pZ = data.pZ;
        player.vX = data.vX;
        player.vY = data.vY;
        player.vZ = data.vZ;
        player.rX = data.rX;
        player.rY = data.rY;
        player.s = data.s;
    }
    changeGun (sessionId: string, data: any) {
        const player = this.players.get(sessionId);        
        player.iGun = data.iGun;
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 4;

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        this.onMessage("move", (client, data) => {            
            this.state.movePlayer(client.sessionId, data);
        });

        this.onMessage("shoot", (client, data) =>{
            this.broadcast("Shoot", data, {except: client});
        });

        this.onMessage("changeGun", (client, data) =>{
            this.state.changeGun(client.sessionId, data);
        });

        this.onMessage("damage", (client, data) =>{
            const clientID = data.id;
            const player = this.state.players.get(clientID);
            const hp = player.currentHP - data.value;
            if (hp > 0)
            {
                player.currentHP = hp;
                return;
            }

            player.team ? this.state.lossA++ : this.state.lossB++;
            const a = this.state.lossA;
            const b = this.state.lossB;
            const score = JSON.stringify({a, b});
            this.broadcast("score", score);

            player.currentHP = player.maxHP;

            for (var i = 0; i < this.clients.length; i++)
            {
                if (this.clients[i].id != clientID) continue;
                
                const x = Math.floor(Math.random() * 50) - 25;
                const z = Math.floor(Math.random() * 50) - 25;

                const message = JSON.stringify({x, z});
                this.clients[i].send("Restart", message);
            }
        });
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client, data: any) {
        client.send("hello", "world");
        this.state.createPlayer(client.sessionId, data);
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }

}
