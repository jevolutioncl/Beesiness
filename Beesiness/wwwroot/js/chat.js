document.addEventListener('DOMContentLoaded', () => {
    const chatContainer = document.getElementById('chatContainer');
    const sendBtn = document.getElementById('sendBtn');
    const newChatBtn = document.getElementById('newChatBtn');
    const promptInput = document.getElementById('prompt');

    // Declarar chatMessages como una variable global
    let chatMessages = [];

    sendBtn.addEventListener('click', async () => {
        const userInput = promptInput.value.trim();
        if (!userInput) return;

        addMessage('user', userInput);
        chatMessages.push({ role: 'user', content: userInput }); // Usar chatMessages aquí
        promptInput.value = '';

        const typingIndicator = addMessage('ai', 'BeeAI está escribiendo...');
        try {
            const response = await fetch('/ApiAI/AskApiAI', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ prompt: userInput, messages: chatMessages })
            });

            if (!response.ok) {
                throw new Error('Error en la comunicación con la IA.');
            }

            const result = await response.json();
            chatContainer.removeChild(typingIndicator);

            if (result && result.messages) {
                chatMessages = result.messages;
                const aiMessage = result.messages.find(msg => msg.role === 'assistant');
                if (aiMessage && aiMessage.content) {
                    addMessage('ai', aiMessage.content);
                } else {
                    addMessage('ai', 'No se recibió una respuesta válida de la IA.');
                }
            } else {
                addMessage('ai', 'Hubo un problema al procesar la respuesta.');
            }
        } catch (error) {
            chatContainer.removeChild(typingIndicator);
            addMessage('ai', 'Hubo un error al procesar tu solicitud. Por favor, intenta nuevamente.');
            console.error(error);
        }
    });

    newChatBtn.addEventListener('click', () => {
        chatMessages = []; // Reiniciar los mensajes
        chatContainer.innerHTML = '<div class="chat-message ai">BeeAI está lista para responder, ¡haz una consulta sobre Apicultura!</div>';
    });

    function addMessage(role, content) {
        const messageDiv = document.createElement('div');
        messageDiv.className = `chat-message ${role}`;
        messageDiv.innerHTML = `<strong>${role === 'user' ? 'Usuario' : 'BeeAI'}:</strong> ${content}`;
        chatContainer.appendChild(messageDiv);
        chatContainer.scrollTop = chatContainer.scrollHeight;
        return messageDiv;
    }
});
