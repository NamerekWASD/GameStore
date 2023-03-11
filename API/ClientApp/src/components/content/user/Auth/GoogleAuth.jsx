//External imports
import { useEffect, useRef } from 'react'
import { LoadScript } from '../../../../utils/LoadScript';

const GoogleAuth = ({ sendToServer }) => {
    const googleButton = useRef(null);

    const src = 'https://accounts.google.com/gsi/client'

    useEffect(() => {
        LoadScript(src)
            .then(() => {
                /*global google*/
                google.accounts.id.initialize({
                    client_id: process.env.REACT_APP_GOOGLE_OAUTH2_CLIENT_ID,
                    callback: handleCredentialResponse
                })
                google.accounts.id.renderButton(
                    googleButton.current,
                    {
                        theme: 'outline',
                        size: 'medium',
                    }
                )
            })
            .catch()

        return () => {
            const scriptTag = document.querySelector(`script[src="${src}"]`)
            if (scriptTag) document.body.removeChild(scriptTag)
        }
    }, [])

    const handleCredentialResponse = async function handleCredentialResponse(r) {
        const newresponse = await fetch(`https://oauth2.googleapis.com/tokeninfo?id_token=${r.credential}`)
        const responseBody = await newresponse.json();
        console.log(responseBody);

        if (!newresponse.ok) {
            console.log(newresponse.status);
        }

        var user = {
            email: responseBody.email,
            userName: responseBody.email,
            ImageURL: responseBody.picture,
        }
        sendToServer("Google", user, responseBody.sub)
    }

    return (
        <>
            <div ref={googleButton}></div>
        </>
    )
}
export default GoogleAuth