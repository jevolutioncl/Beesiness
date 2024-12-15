const chatContainer = document.getElementById("chatContainer");
const userInput = document.getElementById("userInput");

// Reemplaza con tu clave de OpenAI
const openAiApiKey = "INSERTAR_API";

async function sendMessage() {
    const userMessage = userInput.value.trim();
    if (!userMessage) return;

    // Añadir mensaje del usuario al chat
    const userMessageElement = document.createElement("div");
    userMessageElement.className = "text-white p-2 bg-gray-600 rounded-lg mb-2 w-4/5 self-end";
    userMessageElement.textContent = userMessage;
    chatContainer.appendChild(userMessageElement);

    userInput.value = ""; // Limpiar el campo de entrada
    chatContainer.scrollTop = chatContainer.scrollHeight;

    // Mostrar mensaje de escritura
    const aiResponseElement = document.createElement("div");
    aiResponseElement.className = "text-white p-2 bg-gray-800 rounded-lg mb-2 w-4/5";
    aiResponseElement.textContent = "La IA está escribiendo...";
    chatContainer.appendChild(aiResponseElement);

    try {
        // Enviar mensaje directamente a OpenAI
        const response = await fetch("https://api.openai.com/v1/chat/completions", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${openAiApiKey}`,
            },
            body: JSON.stringify({
                model: "gpt-3.5-turbo",
                messages: [
                    { role: "system", content: "Eres un asistente de apicultura que ayuda a responder preguntas sobre apicultura." },
                    { role: "user", content: userMessage },
                ],
            }),
        });

        if (!response.ok) {
            aiResponseElement.textContent = "Error al conectar con la IA.";
            return;
        }

        const data = await response.json();
        const aiResponse = data.choices[0].message.content;

        // Mostrar respuesta de la IA simulando el efecto de escritura
        typeResponse(aiResponse, aiResponseElement);
    } catch (error) {
        aiResponseElement.textContent = "Ocurrió un error al obtener la respuesta de la IA.";
        console.error("Error:", error);
    }
}

function nuevoChat() {
    const confirmacion = confirm("¿Estás seguro de que deseas iniciar un nuevo chat? Esto borrará el historial actual.");
    if (confirmacion) {
        const chatContainer = document.getElementById("chatContainer");
        chatContainer.innerHTML = '<div class="text-white p-2 bg-gray-800 rounded-lg mb-2 w-4/5">BeeAI está lista para responder, ¡haz una consulta sobre Apicultura!</div>';
    }
}

function formatTextAsList(text) {
    const sentences = text.split(/(?<!\.\d)\.\s+/); // Divide en oraciones sin dividir números decimales
    const listItems = [];

    sentences.forEach((sentence) => {
        if (/^\d+\.\s/.test(sentence.trim())) {
            listItems.push(`<li>${sentence.trim()}.</li>`);
        } else {
            listItems.push(`<p>${sentence.trim()}.</p>`);
        }
    });

    if (listItems.some((item) => item.startsWith("<li>"))) {
        return `<ul class="list-disc pl-5 space-y-2 text-white">${listItems.join("")}</ul>`;
    } else {
        return listItems.join("");
    }
}

function typeResponse(text, element) {
    element.innerHTML = ""; // Vacía el contenido del elemento antes de escribir

    const formattedText = formatTextAsList(text); // Formatea el texto como HTML
    let index = 0;

    const typingInterval = setInterval(() => {
        // Inserta HTML progresivamente en lugar de carácter por carácter para evitar mostrar etiquetas HTML sin procesar
        element.innerHTML = formattedText.slice(0, index);
        index++;

        chatContainer.scrollTop = chatContainer.scrollHeight;

        if (index > formattedText.length) {
            clearInterval(typingInterval);
        }
    }, 25);
}
