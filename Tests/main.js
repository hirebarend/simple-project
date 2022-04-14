const axios = require("axios").default;
const uuid = require("uuid");

(async () => {
  let concurrency = 1;

  let longAverage = 500;

  let shortAverage = longAverage;

  while (true) {
    console.log(`concurrency: ${concurrency}`);

    const promises = [];

    for (let i = 0; i < concurrency; i++) {
      promises.push(execute());
    }

    const result = await Promise.all(promises);

    shortAverage = result.reduce((a, b) => a + b) / result.length;

    const ratio = shortAverage / longAverage;

    console.log(`shortAverage: ${shortAverage}`);

    console.log(`ratio: ${ratio}`);

    if (ratio < 1) {
      concurrency += 1;
    } else if (ratio > 1) {
      concurrency -= 1;
    }

    longAverage = shortAverage;
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
        (responseGet.data && responseGet.data.state === 3) ||
        responseGet.data.state === 4 ||
        responseGet.data.state === 5 ||
        responseGet.data.state === 6
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
