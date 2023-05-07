import { toast } from "react-toastify";
import { AppPaths } from "./AppPaths";

/////////////////// User API ////////////////////////////////

export async function SendLoginData(LoginModel) {
    const requestInfo = `api/account/authorize`;
    const requestInit = {
        method: 'POST',
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify(LoginModel)
    };
    return await fetch(requestInfo, requestInit);
}

export async function SendEmailConfirmationCode(confirmationModel) {
    const requestInfo = `api/account/confirm`;
    const requestInit = {
        method: 'POST',
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify(confirmationModel)
    };
    return await fetch(requestInfo, requestInit);
}

export async function CheckAuthenticated() {
    const requestInfo = `api/account`;
    return await sendToServerWithJSONResponse(requestInfo);
};

export const GetUserData = async (refreshAuth) => {
    const response = await fetch('api/account/data', {
        method: "GET",
        headers: {
            "Accept": "application/json",
        }
    })
    if (response.redirected && refreshAuth) {
        refreshAuth();
        return;
    }
    return response;
}

export const logout = async (update) => {
    const response = await fetch('api/account/logout');
    if (response.ok) {
        update();
        toast.info("Ви вийшли з аккаунту");
        return;
    }
    toast.error("Сталася помилка...")
}

export async function requestIsInRole(role) {
    const requestInfo = "api/account/" + role
    return await fetch(requestInfo);
}

/////////////////// Games API ////////////////////////

export async function GetGames(page) {
    const requestInfo = `api/game?${new URLSearchParams([['page', page]])}`;
    return await sendToServerWithJSONResponse(requestInfo);
};
export async function GetGame(id) {
    const requestInfo = "api/game/" + id;
    return await sendToServerWithJSONResponse(requestInfo);
}

export async function GetGenres() {
    const requestInfo = "api/game/genres";
    return await sendToServerWithJSONResponse(requestInfo);
}

export async function subscribeOnGame(gameId, userEmail) {
    const requestInfo = "api/game/subscribe/" + gameId;
    const requestInit = {
        method: 'POST',
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify(userEmail)
    };
    return await sendToServerWithJSONResponse(requestInfo, requestInit);
}

export async function GetGamesByFilters(filter, page) {
    const requestInfo = `api/game/filter?${new URLSearchParams([['page', page]])}`;
    const requestInit = {
        method: 'POST',
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify(filter)
    };

    return await sendToServerWithJSONResponse(requestInfo, requestInit);
}

export async function GetFilterData() {
    const requestInfo = "api/game/filter";
    const requestInit = {
        method: 'GET',
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
    };
    return await sendToServerWithJSONResponse(requestInfo, requestInit);
}

export async function GetGameModel(gameId) {
    const requestInfo = `api/game/model/` + gameId;
    return await fetch(requestInfo);
}

export async function uploadChanges(game) {
    const requestInfo = `api/game/${game.id && game.id !== 0 ? 'edit' : 'create'}`;
    const requestInit = {
        method: game.id && game.id !== 0 ? 'PUT' : 'POST',
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify(game)
    };
    return await fetch(requestInfo, requestInit);
}
export async function uploadImageToServer(file, gameId) {
    const requestInfo = `api/game/upload-image/${gameId}`;
    const requestInit = {
        method: 'POST',
        mode: 'cors',
        body: file
    };
    return await fetch(requestInfo, requestInit);
}

export async function deleteGame(gameId) {
    const requestInfo = `api/game/delete/${gameId}`;
    const requestInit = {
        method: 'DELETE',
    }
    return await fetch(requestInfo, requestInit);
}

/////////////////// Orders API ////////////////////////

export async function GetOrder(orderNumber, useNavigate, navigate) {
    const requestInfo = "api/order/" + orderNumber;
    const response = await fetch(requestInfo);
    if (response.status === 400) {
        toast.info(await response.text());
        navigate('/');
        return;
    }
    if (useNavigate && response.redirected) {
        navigate(AppPaths.login + '?' + new URLSearchParams([['returnURL', (await response.json()).ReturnUrl]]));
        return;
    }
    return await response.json();
}

export async function GetLastBill(navigate) {
    const requestInfo = "api/order/last-bill";
    const response = await fetch(requestInfo);

    if (response.redirected) {
        navigate(AppPaths.login + '?' + new URLSearchParams([['returnURL', (await response.json()).ReturnUrl]]));
        return;
    }

    return await response.json();
}

/////////////////// Copy API ////////////////////////

export async function sendCopyData(data) {
    const requestInfo = `api/copy/create`;
    const requestInit = {
        method: 'POST',
        mode: 'cors',
        headers: {
            'Accept': 'application/json',
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data),
    };
    return await fetch(requestInfo, requestInit);
}

/////////////////// Utils ////////////////////////

async function sendToServerWithJSONResponse(requestInfo, requestInit) {
    const response = await fetch(requestInfo, requestInit)
    return await response.json();
}