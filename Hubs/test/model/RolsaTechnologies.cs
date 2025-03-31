using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace RolsaTechnologies
{
    public class Customer
    {
        [Key]
        [Column(Order=1)]
        public int CID {get;set;}
        [MaxLength(35)]
        public string? Forename {get;set;}
        [MaxLength(35)]
        public string? Surname {get;set;}
        [MaxLength(50)]
        public string? Email {get;set;}
        [Required]
        [MinLength(3)]
        [MaxLength(15)]
        public required string Username {get;set;}
        [Required]
        public required string HashedPassword {get;set;}
        [Required]
        public required string Salt {get;set;}
        [MaxLength(12)]
        public string? PhoneNumber {get;set;}
        public DateTime CreateDate {get;set;} = DateTime.UtcNow;

        public ICollection<ScheduleBooking> ScheduleBooking {get;set;} = new List<ScheduleBooking>();
        public ICollection<ConsultationMessage> ConsultationMessage {get;set;} = new List<ConsultationMessage>();

    }
    
    public class ScheduleType
    {
        [Key]
        [Column(Order=1)]
        public int SID {get;set;}
        [Required]
        public required string Schedule {get;set;}
        public ICollection<ScheduleBooking> ScheduleBooking {get;set;} = new List<ScheduleBooking>();
    }

    public class ScheduleBooking 
    {
        [Key]
        [Column(Order=1)]
        public int SBID {get;set;}
        public int ScheduleTypeID {get;set;}
        public virtual ScheduleType? ScheduleType {get;set;}
        [Required]
        public required string HousePostcode {get;set;}
        public DateOnly Date {get;set;}
        public int Hour {get;set;}
        public int UsernameID {get;set;}
        public int EmailID {get;set;}
        public virtual Customer? Customer {get;set;}
        public DateTime CreateDate {get;set;} = DateTime.UtcNow;

        public ICollection<SolarPanelInstallation> SolarPanelInstallation {get;set;} = new List<SolarPanelInstallation>();
        public ICollection<ConsultationMessage> ConsultationMessage {get;set;} = new List<ConsultationMessage>();
    }

    public class GreenEnergyProduct 
    {
        [Key]
        [Column(Order=1)]
        public int PID {get;set;}
        [Required]
        [MaxLength(100)]
        public required string ProductName {get;set;}
        [Required]
        [MaxLength(2000)]
        public required string Description {get;set;}
        [Required]
        public required string Picture {get;set;}

        public ICollection<LinksForProduct> LinksForProduct {get;set;} = new List<LinksForProduct>();
    }

    public class LinksForProduct 
    {
        [Key]
        [Column(Order=1)]
        public int LID {get;set;}
        public int PID {get;set;}
        public virtual GreenEnergyProduct? GreenEnergyProduct {get;set;}
        public decimal Price {get;set;}
        public int Stock {get;set;}
        [Required]
        public required string Link {get;set;}
        [Required]
        public required string LogoURL {get;set;}
    }
    
    public class SolarPanelInstallation 
    {
        [Key]
        [Column(Order=1)]
        public int SPID {get;set;}
        public int HousePostcodeID {get;set;}
        public virtual ScheduleBooking? ScheduleBooking {get;set;}
        [Required]
        public required string HouseType {get;set;}
        [Required]
        public required int NumOfBedrooms {get;set;}
        [Required]
        public required bool IncludeBattery {get;set;}
    }

    public class ConsultationMessage
    {
        [Key]
        [Column(Order=1)]
        public int CMID {get;set;}
        public int EmailID {get;set;}
        public virtual Customer? Customer {get;set;}
        public int SBID {get;set;}
        public virtual ScheduleBooking? ScheduleBooking {get;set;}
        [Required]
        [MaxLength(1000)]
        public required string Message {get;set;}
    }

    public class Article
    {
        [Key]
        [Column(Order = 1)]
        public int AID { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Description { get; set; }

        [Required]
        public required string Content { get; set; }

        [Required]
        public required string ImageURL { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        
        public string? Author { get; set; }
    }

    public class RolsaTechnologiesContext : DbContext
    {
        public RolsaTechnologiesContext(DbContextOptions<RolsaTechnologiesContext> options) : base(options)
        {
        }

        public required DbSet<Customer> Customer {get;set;}
        public required DbSet<ScheduleType> ScheduleTypes {get;set;}
        public required DbSet<ScheduleBooking> ScheduleBooking {get;set;}
        public required DbSet<GreenEnergyProduct> GreenEnergyProducts {get;set;}
        public required DbSet<LinksForProduct> LinksForProducts {get;set;}
        public required DbSet<SolarPanelInstallation> SolarPanelInstallations {get;set;}
        public required DbSet<ConsultationMessage> ConsultationMessages {get;set;}
        public required DbSet<Article> Articles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique
            modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Username)
            .IsUnique(); // Makes sure that username is unique
            
            modelBuilder.Entity<Customer>()
            .HasIndex(c => c.PhoneNumber)
            .IsUnique(); // Makes sure that phone number is unique


            // Relationships

            //ScheduleBooking
            modelBuilder.Entity<ScheduleBooking>()
            .HasOne(sb => sb.Customer)
            .WithMany(c => c.ScheduleBooking)
            .HasForeignKey(sb => sb.UsernameID);

            modelBuilder.Entity<ScheduleBooking>()
            .HasOne(sb => sb.Customer)
            .WithMany(c => c.ScheduleBooking)
            .HasForeignKey(sb => sb.EmailID);

            modelBuilder.Entity<ScheduleBooking>()
            .HasOne(sb => sb.ScheduleType)
            .WithMany(st => st.ScheduleBooking)
            .HasForeignKey(sb => sb.ScheduleTypeID);

            //ConsultationMessages
            modelBuilder.Entity<ConsultationMessage>()
            .HasOne(cm =>cm.Customer)
            .WithMany(c => c.ConsultationMessage)
            .HasForeignKey(cm => cm.EmailID)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ConsultationMessage>()
            .HasOne(cm =>cm.ScheduleBooking)
            .WithMany(sb => sb.ConsultationMessage)
            .HasForeignKey(cm => cm.SBID)
            .OnDelete(DeleteBehavior.Restrict);

            //SolarPanelInstallation
            modelBuilder.Entity<SolarPanelInstallation>()
            .HasOne(sp => sp.ScheduleBooking)
            .WithMany(sb => sb.SolarPanelInstallation)
            .HasForeignKey (sp => sp.HousePostcodeID);

            modelBuilder.Entity<SolarPanelInstallation>()
            .HasOne(sp => sp.ScheduleBooking)
            .WithMany(sb => sb.SolarPanelInstallation)
            .HasForeignKey (sp => sp.HousePostcodeID);

            modelBuilder.Entity<LinksForProduct>()
            .HasOne(lp => lp.GreenEnergyProduct)
            .WithMany(ge => ge.LinksForProduct)
            .HasForeignKey (lp => lp.PID);

            // Setting the two schedule types
            modelBuilder.Entity<ScheduleType>().HasData(
                new ScheduleType { SID = 1, Schedule = "Installation" },
                new ScheduleType { SID = 2, Schedule = "Consultation" }
            );

            // GreenEnergyProduct
            modelBuilder.Entity<GreenEnergyProduct>().HasData(
                new GreenEnergyProduct
                {
                    PID = 1,
                    ProductName = "Grant Aerona3 R32 Air Source Heat Pump",
                    Description = "Compact in size and highly efficient, the Aerona³ heat pumps provide homeowners with a renewable solution to fulfil their heating and hot water requirements. Using the environmentally friendly R32 refrigerant, an Aerona³ heat pump achieves high performances to effectively keep properties warm while also helping households to lower their dependency on fossil fuels.",
                    Picture = "https://www.theunderfloorheatingstore.com/cdn/shop/files/large-hpid10-10kw-aerona3-heat-pump_8091452703001-336937_700x700.jpg?v=1721297430"
                }
            );

            // LinksForProduct
            modelBuilder.Entity<LinksForProduct>().HasData(
                new LinksForProduct
                {
                    LID = 1,
                    PID = 1,
                    Price = 2838.00m,
                    Link = "https://www.theunderfloorheatingstore.com/products/aerona-inverter-driven-air-source-heat-pump",
                    LogoURL = "https://www.theunderfloorheatingstore.com/cdn/shop/files/the-underfloor-heating-store-hg-logos_1311x250.png?v=1732785944"
                }
            );

            // Articles
            modelBuilder.Entity<Article>().HasData(
            new Article
            {
                /* Content stored in html to be rendered easier. */
                AID = 1,
                Title = "The Future of Solar Energy",
                Description = "A deep dive into how solar energy is shaping the future of sustainable living.",
                Content = @"<h2>Introduction</h2>
                            <p>As the world grapples with the challenges of climate change and dwindling fossil fuel reserves, solar energy stands out as a beacon of hope for a sustainable future. With rapid advancements in technology, decreasing costs, and increasing adoption rates, solar power is poised to revolutionize the global energy landscape. But what does the future hold for solar energy, and how will it shape the way we live?</p>

                            <h2>Advances in Solar Technology</h2>
                            <p>One of the most significant drivers of solar energy's future is technological innovation. Researchers are continuously developing more efficient photovoltaic (PV) cells, allowing for greater energy production from the same amount of sunlight. Emerging technologies such as perovskite solar cells, tandem solar cells, and bifacial panels are pushing efficiency levels beyond 40%, making solar power more viable than ever.</p>

                            <p>Additionally, solar storage solutions are improving, addressing one of the biggest challenges of solar energy—intermittency. Breakthroughs in battery technology, such as solid-state batteries and lithium-sulfur alternatives, are increasing storage capacity and lifespan, making solar energy a reliable round-the-clock power source.</p>

                            <h2>Economic and Environmental Impact</h2>
                            <p>The cost of solar energy has plummeted in recent years, making it one of the most affordable sources of electricity. Government incentives, subsidies, and policies supporting renewable energy have further accelerated its adoption. As more households and businesses transition to solar, dependence on fossil fuels is decreasing, leading to a significant reduction in carbon emissions.</p>

                            <p>Moreover, solar energy reduces strain on power grids and enhances energy independence. Countries with abundant sunlight can harness solar power to become self-sufficient and even export clean energy, fostering a new era of international cooperation in sustainability.</p>

                            <h2>The Role of Solar Energy in Smart Cities</h2>
                            <p>Future cities will integrate solar energy into their very fabric. Smart grids, powered by decentralized solar panels on rooftops, will allow for efficient energy distribution and consumption. Solar-powered infrastructure, from streetlights to transportation systems, will create cleaner and greener urban environments.</p>

                            <p>Additionally, the rise of solar microgrids will provide energy to remote and underserved areas, bringing electricity to communities that previously relied on costly and polluting diesel generators.</p>

                            <h2>Challenges and Potential Solutions</h2>
                            <p>Despite its many advantages, solar energy still faces challenges. Manufacturing solar panels requires significant energy and resources, raising concerns about environmental impact. Recycling initiatives and advancements in sustainable materials aim to address this issue.</p>

                            <p>Another challenge is land use. Large-scale solar farms require vast amounts of space, potentially conflicting with agriculture and conservation efforts. Innovations like agrivoltaics, where solar panels are integrated into farming practices, offer a promising solution.</p>

                            <h2>Conclusion</h2>
                            <p>The future of solar energy is bright. With continued innovation, supportive policies, and growing awareness of the need for clean energy, solar power is set to transform the way we live. As efficiency improves and costs continue to decline, solar energy will play a pivotal role in building a sustainable and resilient future for generations to come.</p>
                            ",
                ImageURL = "https://images.pexels.com/photos/356036/pexels-photo-356036.jpeg",
                CreateDate = new DateTime(2024, 12, 20),
                Author = "John Doe"
            },
            new Article
            {
                AID = 2,
                Title = "Top 5 Renewable Energy Technologies of 2025",
                Description = "Exploring the most innovative renewable energy technologies that are revolutionizing the industry.",
                Content = @"<h2>Introduction</h2>
                            <p>As the world accelerates toward sustainability, renewable energy technologies are playing a pivotal role in reshaping the global energy landscape. With the year 2025 on the horizon, several innovations are set to push the boundaries of what’s possible, offering cleaner, more efficient alternatives to fossil fuels. This article takes a deep dive into the top five renewable energy technologies that are expected to revolutionize the industry in the coming years.</p>

                            <h2>1. Solar Photovoltaics (PV) with Advanced Materials</h2>
                            <p>Solar energy remains the frontrunner in the renewable energy sector, and 2025 will see significant improvements in solar PV technologies. Advances in materials like perovskite solar cells and tandem solar cells promise to increase efficiency and reduce production costs. These new materials allow solar panels to absorb more light, particularly in low-light conditions, resulting in higher energy yields. The integration of bifacial panels, which capture sunlight from both sides, further enhances efficiency, ensuring solar energy continues to lead the renewable charge.</p>

                            <h2>2. Offshore Wind Power</h2>
                            <p>Wind energy has long been a key player in renewable power generation, but the next frontier lies in offshore wind farms. By 2025, offshore wind is poised to become a dominant force, thanks to larger, more powerful turbines and advanced floating platforms. These technologies allow turbines to be placed in deeper waters, unlocking the vast potential of wind energy from areas previously unreachable. Offshore wind farms can produce more consistent energy, benefiting coastal regions and contributing to grid stability.</p>

                            <h2>3. Green Hydrogen</h2>
                            <p>Green hydrogen is gaining momentum as one of the most promising solutions for decarbonizing industries that are hard to electrify, such as steel, cement, and heavy transport. Produced using renewable electricity to split water into hydrogen and oxygen (via electrolysis), green hydrogen produces no carbon emissions. As electrolyzer technology improves and costs continue to fall, green hydrogen will become a critical component of the global energy mix by 2025, supporting energy storage, transportation, and industrial processes.</p>

                            <h2>4. Energy Storage and Solid-State Batteries</h2>
                            <p>Energy storage technology is essential for overcoming one of the biggest challenges in renewable energy: intermittency. Advances in solid-state batteries and other next-generation storage technologies will allow for more efficient, scalable, and longer-lasting energy storage. Solid-state batteries are lighter, safer, and have higher energy densities than conventional lithium-ion batteries, making them ideal for grid-scale applications. This technology will help store excess solar and wind energy for use during periods of low generation, ensuring a reliable and consistent energy supply.</p>

                            <h2>5. Geothermal Energy</h2>
                            <p>Geothermal energy, long considered a niche resource, is experiencing a renaissance with new technologies that enhance its accessibility and efficiency. Enhanced geothermal systems (EGS), which involve injecting water into deep rock formations to generate heat, are expected to make geothermal power a more widespread and viable energy source. By 2025, advancements in drilling technology and reservoir management will unlock geothermal energy’s potential in regions that were once considered unsuitable, offering a constant, reliable energy source for both electricity generation and heating.</p>

                            <h2>Conclusion</h2>
                            <p>The renewable energy revolution is accelerating, with technologies like advanced solar PV, offshore wind, green hydrogen, energy storage, and geothermal energy paving the way for a cleaner, more sustainable future. As these innovations mature, they will work in tandem to reduce global carbon emissions, provide energy security, and shift the world toward a green energy economy. By 2025, these cutting-edge technologies will be key drivers in the transition to a sustainable, renewable-powered world.</p>
                            ",
                ImageURL = "https://images.pexels.com/photos/1108572/pexels-photo-1108572.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
                CreateDate = new DateTime(2025, 1, 2),
                Author = "Sarah Green"
            },
            new Article
            {
                AID = 3,
                Title = "How Homeowners Can Benefit from Solar Panel Installations",
                Description = "Understanding the advantages of solar panel installations for residential homes.",
                Content = @"<h2>Introduction</h2>
                            <p>As the demand for renewable energy continues to rise, solar panels have become an increasingly popular choice for homeowners looking to reduce their environmental impact and save on energy costs. Installing solar panels on your home not only provides a clean, renewable energy source but also offers a host of financial, environmental, and practical benefits. In this article, we’ll explore the advantages that homeowners can enjoy by making the switch to solar energy.</p>

                            <h2>1. Significant Reduction in Energy Bills</h2>
                            <p>One of the most immediate and tangible benefits of installing solar panels is the reduction in electricity bills. By generating your own electricity from the sun, you decrease your reliance on the grid, which can be particularly beneficial during peak demand hours when electricity rates are higher. In many cases, homeowners can save hundreds or even thousands of dollars annually, depending on the size of the system and local electricity prices. Additionally, solar panel systems often come with net metering options, allowing homeowners to sell excess energy back to the grid, further offsetting costs.</p>

                            <h2>2. Increase in Property Value</h2>
                            <p>Homes with solar panel systems are becoming more desirable in the real estate market. A study by the U.S. Department of Energy found that homes with solar installations typically sell for more than homes without them. Buyers are increasingly attracted to the prospect of lower energy costs and the environmental benefits that come with owning a home powered by renewable energy. As a result, installing solar panels not only provides immediate savings but can also boost your home’s long-term resale value.</p>

                            <h2>3. Environmental Impact and Sustainability</h2>
                            <p>Solar energy is one of the cleanest sources of power, with no direct emissions of greenhouse gases or pollutants. By installing solar panels, homeowners significantly reduce their carbon footprint, contributing to a cleaner environment. Over the lifespan of a solar system, homeowners can offset tons of CO2 emissions, helping to fight climate change and reduce air pollution. The shift to solar energy is a key part of the global transition toward sustainability and renewable resources.</p>

                            <h2>4. Energy Independence and Security</h2>
                            <p>Relying on traditional energy sources can leave homeowners vulnerable to power outages and price fluctuations. Solar panel installations, particularly when paired with energy storage systems like batteries, offer a way to become more energy independent. With solar power, homeowners can generate their own electricity and store excess energy for use during cloudy days or at night. In areas prone to natural disasters or grid instability, this can provide peace of mind and greater energy security.</p>

                            <h2>5. Incentives and Tax Benefits</h2>
                            <p>Homeowners can take advantage of various financial incentives and tax credits designed to make solar energy more affordable. In many countries, the government offers rebates, tax credits, and grants to reduce the upfront costs of solar panel installation. In the U.S., for instance, the federal solar tax credit allows homeowners to deduct a percentage of the installation costs from their federal taxes, providing significant savings. Many states also offer additional incentives and rebates, making solar energy even more accessible for residential properties.</p>

                            <h2>6. Low Maintenance and Longevity</h2>
                            <p>Once installed, solar panels require minimal maintenance. With no moving parts, they are incredibly durable and can last 25 years or more with proper care. Regular cleaning and occasional inspections are typically all that’s needed to keep the system running efficiently. Additionally, most solar panels come with warranties that ensure performance for decades, offering homeowners peace of mind about the longevity of their investment.</p>

                            <h2>7. Supporting Green Jobs and the Economy</h2>
                            <p>The growth of the solar energy industry has created a multitude of new jobs, from manufacturing to installation and maintenance. By investing in solar panels, homeowners are not only benefiting from cleaner energy but also contributing to the growth of the green economy. This helps create local job opportunities and stimulates innovation in renewable technologies, further supporting the transition to a sustainable future.</p>

                            <h2>Conclusion</h2>
                            <p>Solar panel installations offer a wide range of benefits for homeowners, from reducing energy bills and increasing property value to supporting a cleaner environment. With incentives and tax benefits making solar energy more affordable than ever, there has never been a better time for homeowners to make the switch. By investing in solar panels, homeowners are not only securing a more sustainable and cost-effective future for themselves but also contributing to a greener, cleaner planet for generations to come.</p>
                            ",
                ImageURL = "https://images.pexels.com/photos/8853502/pexels-photo-8853502.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
                CreateDate = new DateTime(2025, 1, 3),
                Author = "Michael Brown"
            },
            new Article
            {
                AID = 4,
                Title = "Wind Energy: A Key Player in the Renewable Revolution",
                Description = "How wind energy is contributing to global efforts in reducing carbon footprints.",
                Content = @"<h2>Introduction</h2>
                            <p>As the world races to combat climate change, wind energy has emerged as one of the most promising and efficient renewable energy sources. With its ability to generate large amounts of electricity without emitting carbon dioxide, wind energy is playing a pivotal role in the global shift toward cleaner, more sustainable energy systems. This article delves into how wind energy is contributing to the fight against climate change and reducing global carbon footprints.</p>

                            <h2>1. A Clean and Renewable Energy Source</h2>
                            <p>Wind energy is inherently clean, producing no emissions during the electricity generation process. Unlike fossil fuel-based power plants, which release harmful pollutants like carbon dioxide and sulfur dioxide, wind turbines harness the power of the wind to generate electricity, leaving no lasting environmental impact. This makes wind energy one of the most sustainable and environmentally friendly energy sources available today.</p>

                            <p>By shifting from fossil fuels to wind energy, countries can significantly reduce their carbon footprints and work toward achieving climate goals outlined in international agreements like the Paris Climate Accord. The widespread adoption of wind power has the potential to reduce global greenhouse gas emissions substantially.</p>

                            <h2>2. Wind Power's Rapid Growth and Global Potential</h2>
                            <p>Wind energy has experienced significant growth over the past few decades. In 2024, wind power accounted for nearly 10% of global electricity generation, with more than 900 gigawatts (GW) of installed capacity worldwide. This growth is set to continue, with some countries aiming to have 50% or more of their electricity needs met by wind power by 2030.</p>

                            <p>The potential for wind energy to further expand is vast. According to studies, wind could supply up to 40% of the world’s electricity needs by 2050, provided governments continue to invest in infrastructure, innovation, and policies that support renewable energy development. Offshore wind farms, in particular, hold enormous promise, with vast untapped energy potential in coastal regions where wind speeds are highest and most consistent.</p>

                            <h2>3. Economic Benefits and Job Creation</h2>
                            <p>The wind energy industry is also driving economic growth and job creation around the world. The transition to wind power requires skilled workers for manufacturing, installation, and maintenance, leading to the creation of thousands of new jobs. In fact, the renewable energy sector, including wind power, is one of the fastest-growing industries globally, outpacing fossil fuel industries in job creation.</p>

                            <p>In countries like Denmark, Germany, and the United States, wind energy has already become a major contributor to the economy. As the technology continues to improve and the demand for clean energy rises, the wind energy sector will likely provide even more economic benefits, including job opportunities in local communities and new innovations in energy storage and grid management.</p>

                            <h2>4. Wind Power and Energy Independence</h2>
                            <p>Wind energy also contributes to energy independence by reducing reliance on imported fossil fuels. Countries with abundant wind resources can harness this natural power to generate electricity domestically, decreasing their dependence on foreign energy sources. This not only enhances energy security but also stabilizes national economies by mitigating the volatility of global fuel prices.</p>

                            <p>Moreover, decentralized wind energy systems—especially small-scale turbines—offer communities and businesses the ability to produce their own energy, further promoting energy autonomy and reducing the impact of grid outages or disruptions.</p>

                            <h2>5. Reducing the Environmental Impact of Energy Generation</h2>
                            <p>The transition to wind energy plays a crucial role in reducing the overall environmental impact of energy production. In addition to minimizing greenhouse gas emissions, wind power helps reduce air and water pollution that is commonly associated with fossil fuel combustion. Unlike coal or natural gas plants, wind turbines do not require significant amounts of water for cooling, which can have adverse environmental effects in water-scarce regions.</p>

                            <p>In the long run, wind energy can significantly help in the decarbonization of the global energy grid, providing cleaner electricity to industries, homes, and transportation networks, while contributing to the preservation of ecosystems and biodiversity.</p>

                            <h2>6. Technological Advancements and Efficiency Improvements</h2>
                            <p>Recent advancements in wind turbine technology have made wind energy more efficient and cost-effective than ever before. Modern wind turbines are larger, more durable, and capable of harnessing energy from even lower wind speeds. Offshore wind farms, in particular, are becoming more sophisticated with floating turbine technology, which allows turbines to be placed in deeper waters, far from shore.</p>

                            <p>These technological innovations are helping to drive down the cost of wind energy generation, making it more competitive with other forms of electricity. As costs continue to fall, wind energy is expected to become an increasingly important part of the global energy mix.</p>

                            <h2>Conclusion</h2>
                            <p>Wind energy is undoubtedly a key player in the renewable revolution, offering a sustainable, environmentally friendly solution to the challenges of climate change. By harnessing the power of the wind, countries can significantly reduce their carbon footprints, contribute to energy independence, and create economic opportunities through job creation. As technology continues to improve and costs continue to fall, wind energy will become an even more critical component of the global transition to a clean energy future.</p>
                            ",
                ImageURL = "https://images.pexels.com/photos/414837/pexels-photo-414837.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
                CreateDate = new DateTime(2025, 1, 29),
                Author = "Emily Davis"
            },
            new Article
            {
                AID = 5,
                Title = "Understanding the Role of Battery Storage in Renewable Energy",
                Description = "Battery storage is revolutionizing how we store and use renewable energy.",
                Content = @"<h2>Introduction</h2>
                            <p>As the world continues its shift toward renewable energy, one of the most important advancements enabling this transition is battery storage. While renewable sources like solar and wind energy have enormous potential, they are intermittent—meaning they only generate electricity when the sun is shining or the wind is blowing. Battery storage systems are playing a pivotal role in overcoming this challenge, allowing us to store excess energy and use it when demand is high or production is low. In this article, we will explore how battery storage is revolutionizing the renewable energy landscape and enabling a more reliable, sustainable energy future.</p>

                            <h2>1. The Importance of Energy Storage in Renewable Systems</h2>
                            <p>Renewable energy sources such as solar and wind are dependent on weather conditions, meaning their availability fluctuates throughout the day and year. Without effective energy storage solutions, this intermittency would limit the viability of these renewable sources as reliable power generators. Battery storage systems offer the solution, allowing excess energy to be stored during peak generation times (such as sunny afternoons or windy nights) and drawn upon when demand outstrips supply, such as during cloudy days or at night.</p>

                            <p>Battery storage provides grid operators with the flexibility needed to balance supply and demand, stabilizing the grid and ensuring a continuous, reliable flow of electricity. This is especially important as more regions transition to 100% renewable energy grids in their efforts to reduce greenhouse gas emissions.</p>

                            <h2>2. Types of Battery Storage Systems</h2>
                            <p>There are several types of battery storage systems used in renewable energy applications, each with different characteristics and uses. The most common types include:</p>

                            <ul>
                                <li><strong>Lithium-ion Batteries:</strong> These are the most widely used storage technology, known for their high energy density, efficiency, and relatively low cost. Lithium-ion batteries are commonly used for both residential solar systems and large-scale grid storage. Their ability to charge and discharge rapidly makes them ideal for balancing supply and demand.</li>
                                <li><strong>Flow Batteries:</strong> These batteries store energy in liquid electrolytes and offer advantages in terms of scalability and long-duration storage. While they are less common than lithium-ion batteries, flow batteries have the potential to provide large-scale storage solutions that can discharge for hours or even days, making them ideal for long-term energy storage needs.</li>
                                <li><strong>Sodium-ion Batteries:</strong> Emerging as a potential alternative to lithium-ion batteries, sodium-ion batteries use sodium instead of lithium, offering a more abundant and potentially less expensive resource. Though they are still in the development phase, sodium-ion batteries may become more widely used for large-scale energy storage in the coming years.</li>
                            </ul>

                            <p>Each of these technologies plays a unique role in energy storage, and their development is essential to ensuring that renewable energy can be efficiently stored and used when needed.</p>

                            <h2>3. Enabling a 100% Renewable Energy Grid</h2>
                            <p>One of the most transformative impacts of battery storage is its ability to facilitate the transition to a 100% renewable energy grid. Traditionally, electricity grids have relied on fossil fuel power plants to provide consistent and reliable energy. However, the integration of battery storage with renewable energy sources can provide the same level of reliability without the need for fossil fuels.</p>

                            <p>By storing excess renewable energy in batteries, grids can operate with higher levels of solar and wind power, reducing the need for coal and natural gas plants. This can help to significantly reduce carbon emissions and promote cleaner, greener electricity systems. Additionally, large-scale battery storage systems can act as a backup power source in the event of grid disruptions or emergencies, ensuring a reliable energy supply even during periods of high demand or low generation.</p>

                            <h2>4. The Role of Battery Storage in Electric Vehicles (EVs)</h2>
                            <p>Battery storage is also playing a key role in the rise of electric vehicles (EVs). As the global shift to electric transportation accelerates, the demand for high-capacity batteries is increasing. In fact, EV batteries are often based on similar technologies to those used for grid-scale energy storage systems.</p>

                            <p>EVs are also becoming a valuable tool in balancing electricity grids. Through a process known as vehicle-to-grid (V2G) technology, electric vehicles can be used as mobile energy storage units, supplying stored electricity back to the grid during peak demand periods. This creates a decentralized energy storage system, where individual EV owners can contribute to grid stability and potentially earn money by providing stored energy.</p>

                            <h2>5. Challenges and Future Prospects</h2>
                            <p>While battery storage systems are helping to unlock the full potential of renewable energy, there are still challenges to overcome. One of the primary obstacles is cost—battery storage technologies, particularly for large-scale applications, remain expensive. However, prices are steadily decreasing as technology advances, and economies of scale come into play.</p>

                            <p>Another challenge is the environmental impact of battery production and disposal. While batteries offer many benefits in terms of energy storage, the materials used to create them—such as lithium, cobalt, and nickel—can have environmental and social implications. As battery technologies continue to evolve, researchers are working on ways to make batteries more sustainable and develop recycling methods to mitigate these issues.</p>

                            <h2>6. The Future of Battery Storage in Renewable Energy</h2>
                            <p>Looking ahead, battery storage is expected to become even more integral to the renewable energy landscape. Innovations in battery chemistry, such as solid-state batteries, could improve energy density and efficiency, making storage even more cost-effective. Moreover, the growing adoption of renewable energy technologies like wind, solar, and hydro, combined with energy storage solutions, will pave the way for a more sustainable and resilient energy grid.</p>

                            <p>Furthermore, the ongoing development of smart grid systems, which enable real-time monitoring and optimization of energy use, will enhance the efficiency of battery storage systems and help balance supply and demand more effectively.</p>

                            <h2>Conclusion</h2>
                            <p>Battery storage is revolutionizing how we store and use renewable energy, providing a critical solution to the challenges of intermittency and grid stability. As the technology continues to advance and costs decline, battery storage will play a crucial role in enabling the widespread adoption of renewable energy sources and creating a cleaner, more sustainable energy future. With the potential to support everything from residential solar systems to large-scale grid applications, battery storage is a cornerstone of the renewable energy revolution.</p>
                            ",
                ImageURL = "https://reports.electricinsights.co.uk/wp-content/uploads/2022/09/AdobeStock_248626760-scaled.jpeg",
                CreateDate = new DateTime(2025, 2, 17),
                Author = "Robert Wilson"
            },
            new Article
            {
                AID = 6,
                Title = "Hydroelectric Power: Is It the Future?",
                Description = "Exploring the potential of hydroelectric power as a sustainable energy source.",
                Content = @"<h2>Introduction</h2>
                            <p>As the world transitions toward renewable energy to address climate change, hydroelectric power remains a key player in the pursuit of clean energy. Although hydroelectric systems have been in use for centuries, their potential to contribute to a sustainable energy future is still significant. In this article, we will explore the role of hydroelectric power in our energy landscape and its potential as a long-term solution for reducing carbon emissions and supporting global sustainability.</p>

                            <h2>How Hydroelectric Power Works</h2>
                            <p>Hydroelectric power generates electricity by harnessing the energy of flowing or falling water. The most common method involves building a dam to create a reservoir where water is stored. When the water is released, it flows through turbines that spin and generate electricity. There are also run-of-river systems, which divert water without requiring large dams, as well as pumped-storage systems, which store energy by pumping water uphill during periods of low demand and releasing it to generate power when the demand is higher.</p>

                            <p>Hydroelectric plants can be highly efficient and provide a steady supply of electricity, often operating at full capacity for decades.</p>

                            <h2>The Benefits of Hydroelectric Power</h2>
                            <p>One of the most significant advantages of hydroelectric power is that it is a renewable energy source, utilizing the natural water cycle. Unlike fossil fuels, hydroelectric power does not produce direct greenhouse gas emissions, making it a clean energy option. Furthermore, it provides reliable and flexible power generation, as water is a consistent resource, and hydroelectric plants can quickly adjust output to meet demand.</p>

                            <p>In addition, hydroelectric plants have long lifespans and require relatively low maintenance. Their ability to provide large-scale energy storage through pumped storage systems is another unique feature, allowing excess electricity to be stored and released during peak demand times.</p>

                            <p>Hydroelectric power also helps stabilize energy grids. The reliability of hydropower makes it an ideal complement to intermittent renewable sources like solar and wind, balancing the fluctuations that come with their variability.</p>

                            <h2>Challenges Facing Hydroelectric Power</h2>
                            <p>Despite its many benefits, hydroelectric power faces challenges, particularly related to environmental and social impacts. Large dams can disrupt ecosystems by altering the natural flow of rivers, which affects aquatic life and fish migration patterns. The construction of reservoirs can also displace communities and wildlife.</p>

                            <p>Moreover, the cost and time involved in developing new hydroelectric projects can be substantial. Many of the best sites for new dams are already being used, and new projects often face strong opposition due to environmental concerns.</p>

                            <p>The effects of climate change present another challenge. Hydropower relies on predictable water cycles, but droughts, reduced rainfall, and changes in snowmelt patterns can decrease the amount of water available for electricity generation, which affects its consistency and output.</p>

                            <h2>The Future of Hydroelectric Power</h2>
                            <p>While large-scale hydropower projects may face limitations due to environmental concerns and high upfront costs, the future of hydroelectric power is not bleak. Technological advancements are making it possible to improve the efficiency and sustainability of existing plants, such as upgrading turbines and incorporating fish-friendly technology. There is also growing interest in smaller-scale hydroelectric projects, which reduce environmental impact while still providing reliable energy.</p>

                            <p>Many countries are focusing on modernizing their existing hydroelectric infrastructure rather than building new dams. These efforts often involve refurbishing older plants with new technologies that allow for better efficiency and greater energy output without the need for significant new environmental disruption.</p>

                            <p>In addition, regional potential remains high. Countries with abundant water resources, such as Canada, Brazil, and Scandinavian nations, are well-positioned to continue benefiting from hydroelectric power as part of their broader renewable energy strategies.</p>

                            <h2>Hydroelectric Power's Role in Global Climate Goals</h2>
                            <p>Hydropower plays a significant role in reducing global carbon emissions and addressing climate change. Globally, hydropower accounts for a substantial portion of renewable electricity generation. It is a cornerstone of clean energy in many regions, with countries relying heavily on hydroelectric plants to meet their electricity needs while avoiding the harmful emissions associated with fossil fuels.</p>

                            <p>By continuing to expand and innovate within the hydroelectric sector, hydropower could play an even larger role in achieving global climate goals. The flexibility of hydropower, especially when paired with other renewable energy sources like wind and solar, can help balance intermittent energy generation, making it a critical component of a clean, reliable energy grid.</p>

                            <h2>Conclusion</h2>
                            <p>Hydroelectric power remains one of the most established and effective renewable energy sources available. While it faces challenges, including environmental concerns and the need for substantial investment in infrastructure, its potential for sustainable energy generation is undeniable. With continued technological advancements, modernization of existing plants, and a focus on minimizing ecological impacts, hydroelectric power could play a pivotal role in shaping the future of clean energy, contributing to the global effort to reduce carbon footprints and combat climate change.</p>
                            ",
                ImageURL = "https://images.pexels.com/photos/30707840/pexels-photo-30707840/free-photo-of-scenic-view-of-pitlochry-dam-in-scotland.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
                CreateDate = new DateTime(2025, 3, 23),
                Author = "Jessica Lee"
            }
            );
        }
    }

}