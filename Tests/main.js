const axios = require("axios").default;
const uuid = require("uuid");

(async () => {
  const timestamp1 = new Date().getTime();

  //   for (let i = 0; i < 50; i++) { // 166
  //     await execute();
  //   }

  const array = [];

  for (let i = 0; i < 10; i++) {
    // 18
    array.push(execute());
  }

  await Promise.all(array);

  const timestamp2 = new Date().getTime();

  console.log(`${(timestamp2 - timestamp1) / 1000} seconds`);
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
        console.log(
          `${
            new Date(responseGet.data.updated).getTime() -
            new Date(responseGet.data.created).getTime()
          }`
        );

        break;
      }

      await new Promise((resolve) => setTimeout(() => resolve(), 100));
    }
  } catch {
    console.log(`[ERROR]`);
  }
}
