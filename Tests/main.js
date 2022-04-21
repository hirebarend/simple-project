const axios = require("axios").default;
const uuid = require("uuid");

(async () => {
  const durationInSeconds = 60;

  const requestsPerSecond = 5;

  const numberOfRequests = durationInSeconds * requestsPerSecond;

  console.log(`numberOfRequests: ${numberOfRequests}`);

  const interval = 1000 / requestsPerSecond;

  console.log(`interval: ${interval}`);

  let sum = 0;

  for (let i = 0; i < numberOfRequests; i++) {
    execute().then((x) => {
      sum += x;
    });

    await new Promise((resolve) => setTimeout(() => resolve(), interval));
  }
})();

async function execute() {
  try {
    const reference = uuid.v4();

    const responsePost = await axios.post(
      `https://function-app-4414.azurewebsites.net/api/orders/${reference}`
    );

    while (true) {
      const responseGet = await axios.get(
        `https://function-app-4414.azurewebsites.net/api/orders/${responsePost.data.order.reference}`
      );

      if (
        responseGet.data &&
        (responseGet.data.state === 3 || responseGet.data.state === 4)
      ) {
        const duration =
          new Date(responseGet.data.updated).getTime() -
          new Date(responseGet.data.created).getTime();

        console.log(`duration: ${duration}`);

        return duration;
      }

      await new Promise((resolve) => setTimeout(() => resolve(), 100));
    }
  } catch(error) {
    console.log(`[ERROR] - ${error.message}`);

    return 0;
  }
}
