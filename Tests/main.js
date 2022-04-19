const axios = require("axios").default;
const uuid = require("uuid");

(async () => {
  const durationInSeconds = 2;

  const requestsPerSecond = 5;

  const numberOfRequests = durationInSeconds * requestsPerSecond;

  console.log(`numberOfRequests: ${numberOfRequests}`);

  const interval = requestsPerSecond / 1000;

  console.log(`interval: ${interval}`);

  for (let i = 0; i < numberOfRequests; i++) {
    execute();

    await new Promise((resolve) => setTimeout(() => resolve(), interval));
  }
})();

async function execute() {
  try {
    const reference = uuid.v4();

    const responsePost = await axios.post(
      `https://function-app-5695.azurewebsites.net/api/Order/${reference}`
    );

    while (true) {
      const responseGet = await axios.get(
        `https://function-app-5695.azurewebsites.net/api/Order/${reference}`
      );

      if (
        responseGet.data &&
        (responseGet.data.state === 3 || responseGet.data.state === 4)
      ) {
        return (
          new Date(responseGet.data.updated).getTime() -
          new Date(responseGet.data.created).getTime()
        );
      }

      await new Promise((resolve) => setTimeout(() => resolve(), 100));
    }
  } catch {
    console.log(`[ERROR]`);
  }
}
